using System.Runtime.Serialization;

using System.IO;

namespace RDI.MVC.Models
{
    public abstract class BaseModel : IModel
    {
        public int Id { get; set; }

        public string ToXml()
        {
            string xml;

            using (var memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(GetType());
                serializer.WriteObject(memStm, this);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    xml = streamReader.ReadToEnd();

                }
            }


            return xml;
        }
    }
}
