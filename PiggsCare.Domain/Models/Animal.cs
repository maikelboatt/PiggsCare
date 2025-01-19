namespace PiggsCare.Domain.Models
{
    public class Animal
    {
        public int AnimalId { get; init; }
        public required int Name { get; init; }
        public required string Breed { get; init; }
        public required DateTime BirthDate { get; init; }
        public required int CertificateNumber { get; init; }

        public override string ToString()
        {
            return $"Animal Id: {AnimalId}, Animal Name: {Name}, Breed: {Breed}, BirthDate: {BirthDate}, CertificateNumber: {CertificateNumber}";
        }
    }
}
