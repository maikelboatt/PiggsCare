using Dapper;
using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.DataAccess.Helpers;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;
using System.Data;

namespace PiggsCare.DataAccess.Repositories.ScheduledNotifications
{
    /// <summary>
    ///     Repository for managing scheduled notifications in the database.
    /// </summary>
    public class ScheduledNotificationRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IScheduledNotificationRepository
    {
        /// <summary>
        ///     Retrieves scheduled notifications for a specific date.
        /// </summary>
        /// <param name="scheduledDate" >The date to filter notifications by.</param>
        /// <returns>A collection of <see cref="ScheduledNotification" /> objects scheduled for the given date.</returns>
        public async Task<IEnumerable<ScheduledNotification>> GetScheduledNotificationsByExactDateAsync( DateOnly scheduledDate )
        {
            IEnumerable<ScheduledNotificationDto> notificationResults = await dataAccess.QueryAsync<ScheduledNotificationDto, dynamic>(
                "sp.ScheduledNotificationsByExactDate_Get",
                new { ScheduledDate = dateConverterService.GetDateTime(scheduledDate) }
            ) ?? [];

            List<ScheduledNotification> notifications = [];
            foreach (ScheduledNotificationDto notificationDto in notificationResults)
            {
                IEnumerable<int> animalIds = await dataAccess.QueryAsync<int, dynamic>(
                    "sp.ScheduledNotificationAnimalIds_Get",
                    new { ScheduledNotificationId = notificationDto.Id }
                );

                notifications.Add(
                    new ScheduledNotification(
                        notificationDto.Id,
                        notificationDto.Message,
                        dateConverterService.GetDateOnly(notificationDto.ScheduledDate),
                        animalIds.ToList(),
                        notificationDto.SynchronizationId
                    ));
            }
            return notifications;
        }

        /// <summary>
        ///     Creates a new scheduled notification in the database.
        /// </summary>
        /// <param name="notification" >The notification to create.</param>
        /// <returns>The ID of the newly created notification.</returns>
        public async Task<int> CreateScheduledNotificationAsync( ScheduledNotification notification )
        {
            DataTable animalIdTable = SqlTableHelpers.ToIntListDataTable(notification.AnimalIds);

            DynamicParameters parameters = new();
            parameters.Add("@Message", notification.Message);
            parameters.Add("@ScheduledDate", dateConverterService.GetDateTime(notification.ScheduledDate));
            parameters.Add("@AnimalIds", animalIdTable.AsTableValuedParameter("IntList"));
            parameters.Add("@SynchronizationId", notification.SynchronizationId);

            IEnumerable<int> result = await dataAccess.QueryAsync<int, dynamic>(
                "sp.ScheduledNotification_Insert",
                parameters
            );
            return result.Single();
        }

        /// <summary>
        ///     Updates an existing scheduled notification in the database.
        /// </summary>
        /// <param name="notification" >The notification with updated values.</param>
        public async Task UpdateScheduledNotificationAsync( ScheduledNotification notification )
        {
            DataTable animalIdTable = SqlTableHelpers.ToIntListDataTable(notification.AnimalIds);

            DynamicParameters parameters = new();
            parameters.Add("@Id", notification.Id);
            parameters.Add("@Message", notification.Message);
            parameters.Add("@ScheduledDate", dateConverterService.GetDateTime(notification.ScheduledDate));
            parameters.Add("@AnimalIds", animalIdTable.AsTableValuedParameter("IntList"));
            parameters.Add("@SynchronizationId", notification.SynchronizationId);

            await dataAccess.CommandAsync(
                "UpdateScheduledNotification",
                parameters
            );
        }

        /// <summary>
        ///     Deletes a scheduled notification from the database.
        /// </summary>
        /// <param name="id" >The ID of the notification to delete.</param>
        public async Task DeleteScheduledNotificationAsync( int id )
        {
            await dataAccess.CommandAsync(
                "DeleteScheduledNotification",
                new { Id = id }
            );
        }
    }
}
