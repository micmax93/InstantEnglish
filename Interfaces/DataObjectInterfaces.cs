namespace ElFartas.InstantEnglish.Interfaces
{
    public interface IItem
    {
        int Id { get; set; }
        string Name { get; set; }
        byte[] Image { get; set; }
        int CategoryId { get; set; }
    }

    public interface ICategory
    {
        int Id { get; set; }
        string Name { get; set; }
        int Difficulty { get; set; }
    }

    public interface IWord
    {
        int Id { get; set; }
        string Text { get; set; }
        string Language { get; set; }
        int ItemId { get; set; }
    }

    public interface IUser
    {
        int Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
    public interface IStat
    {
        int Id { get; set; }
        int UserId { get; set; }
        int ItemId { get; set; }
        int Correct { get; set; }
        int Failed { get; set; }
    }
}
