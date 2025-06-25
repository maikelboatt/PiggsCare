namespace PiggsCare.Domain.Models
{
    /// <summary>
    ///     Represents an animal with identification, breed, birthdate, certificate, gender, and back fat index.
    /// </summary>
    /// <param name="animalId" >The unique identifier for the animal.</param>
    /// <param name="name" >The name of the animal.</param>
    /// <param name="breed" >The breed of the animal.</param>
    /// <param name="birthDate" >The birthdate of the animal.</param>
    /// <param name="certificateNumber" >The certificate number of the animal.</param>
    /// <param name="gender" >The gender of the animal.</param>
    /// <param name="backFatIndex" >The back fat index of the animal.</param>
    public class Animal(
        int animalId,
        int name,
        string breed,
        DateOnly birthDate,
        int certificateNumber,
        string gender,
        float backFatIndex )
    {
        /// <summary>
        ///     Gets the unique identifier for the animal.
        /// </summary>
        public int AnimalId { get; init; } = animalId;

        /// <summary>
        ///     Gets the name of the animal.
        /// </summary>
        public int Name { get; init; } = name;

        /// <summary>
        ///     Gets the breed of the animal.
        /// </summary>
        public string Breed { get; init; } = breed;

        /// <summary>
        ///     Gets the birth date of the animal.
        /// </summary>
        public DateOnly BirthDate { get; init; } = birthDate;

        /// <summary>
        ///     Gets the certificate number of the animal.
        /// </summary>
        public int CertificateNumber { get; init; } = certificateNumber;

        /// <summary>
        ///     Gets the gender of the animal.
        /// </summary>
        public string Gender { get; init; } = gender;

        /// <summary>
        ///     Gets the back fat index of the animal.
        /// </summary>
        public float BackFatIndex { get; init; } = backFatIndex;

        /// <summary>
        ///     Gets or sets a value indicating whether the animal is selected.
        /// </summary>
        public bool IsSelected { get; set; } = false;

        /// <summary>
        ///     Returns a string representation of the animal.
        /// </summary>
        /// <returns>A string describing the animal.</returns>
        public override string ToString() =>
            $"Animal Id: {AnimalId}, Animal Name: {Name}, Breed: {Breed}, BirthDate: {BirthDate}, CertificateNumber: {CertificateNumber}, Gender: {Gender}, BackFatIndex: {BackFatIndex}";

        /// <summary>
        ///     Determines whether the specified object is equal to the current animal.
        /// </summary>
        /// <param name="obj" >The object to compare with the current animal.</param>
        /// <returns>
        ///     true if the specified object is an Animal and has the same AnimalId; otherwise, false.
        /// </returns>
        public override bool Equals( object? obj )
        {
            if (obj is not Animal other)
                return false;

            return AnimalId == other.AnimalId;
        }

        /// <summary>
        ///     Returns a hash code for the current animal.
        /// </summary>
        /// <returns>A hash code for the current animal.</returns>
        public override int GetHashCode() => AnimalId.GetHashCode();
    }
}
