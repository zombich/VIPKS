using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lab6MongoDB.Models
{
    public class Book
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("author")]
        public string? Author { get; set; }

        [BsonElement("publication_year")]
        public int? PublicationYear { get; set; }

        [BsonElement("genre")]
        public string? Genre { get; set; }

        [BsonElement("rating")]
        public double? Rating { get; set; }

        [BsonElement("is_available")]
        public bool? IsAvailable { get; set; }

        public override string ToString()
        {
            return $"{Id} - Автор: {Author} - {Name}," +
            $" Жанр - {Genre}, Год публикации - {PublicationYear}," +
            $" Рейтинг - {Rating}, Доступность - {IsAvailable}";
        }
    }
}
