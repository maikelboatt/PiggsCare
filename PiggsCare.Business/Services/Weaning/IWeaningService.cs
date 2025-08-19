using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Weaning
{
    public interface IWeaningService
    {
        /// <summary>
        ///     Retrieves all weaning events for a given identifier.
        /// </summary>
        /// <param name="id" >The identifier to filter weaning events.</param>
        /// <returns>A collection of <see cref="WeaningEvent" />.</returns>
        Task GetAllWeaningEventAsync( int id );

        /// <summary>
        ///     Retrieves a specific weaning event by its identifier.
        /// </summary>
        /// <param name="id" >The identifier of the weaning event.</param>
        /// <returns>The <see cref="WeaningEvent" /> if found; otherwise, null.</returns>
        WeaningEvent? GetWeaningEventByIdAsync( int id );

        /// <summary>
        ///     Creates a new weaning event.
        /// </summary>
        /// <param name="weaning" >The weaning event to create.</param>
        Task CreateWeaningEventAsync( WeaningEvent weaning );

        /// <summary>
        ///     Updates an existing weaning event.
        /// </summary>
        /// <param name="weaning" >The weaning event to update.</param>
        Task UpdateWeaningEventAsync( WeaningEvent weaning );

        /// <summary>
        ///     Deletes a weaning event by its identifier.
        /// </summary>
        /// <param name="weaningId" >The identifier of the weaning event to delete.</param>
        Task DeleteWeaningEventAsync( int weaningId );
    }
}
