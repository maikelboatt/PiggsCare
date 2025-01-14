namespace PiggsCare.Domain.Models
{
    public class Animal(
        int animalId,
        int animalIdentification,
        string breed,
        DateTime birthDate,
        string certificateNumber,
        Gender gender,
        float? backFatIndex )
    {
        public int AnimalId { get; init; } = animalId;
        public int AnimalIdentification { get; init; } = animalIdentification;
        public string Breed { get; set; } = breed;
        public DateTime BirthDate { get; init; } = birthDate;
        public string CertificateNumber { get; set; } = certificateNumber;
        public Gender Gender { get; init; } = gender;
        public float? BackFatIndex { get; set; } = backFatIndex;
    }
}
