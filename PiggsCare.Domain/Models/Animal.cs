namespace PiggsCare.Domain.Models
{
    public class Animal(
        int animalId,
        int name,
        string breed,
        DateOnly birthDate,
        int certificateNumber,
        string gender,
        float backFatIndex )
    {
        public int AnimalId { get; init; } = animalId;
        public int Name { get; init; } = name;
        public string Breed { get; init; } = breed;
        public DateOnly BirthDate { get; init; } = birthDate;
        public int CertificateNumber { get; init; } = certificateNumber;
        public string Gender { get; init; } = gender;
        public float BackFatIndex { get; init; } = backFatIndex;

        public override string ToString()
        {
            return
                $"Animal Id: {AnimalId}, Animal Name: {Name}, Breed: {Breed}, BirthDate: {BirthDate}, CertificateNumber: {CertificateNumber}, Gender: {Gender}, BackFatIndex: {BackFatIndex}";
        }
    }
}
