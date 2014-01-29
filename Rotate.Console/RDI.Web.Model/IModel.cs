namespace RDI.Web.Model
{
    public interface IModel
    {
        int Id { get; set; }
        string ToXml();
    }
}
