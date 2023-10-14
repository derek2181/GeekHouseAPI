namespace GeeekHouseAPI.Models
{
    public class GenericResponse<T>
    {
        public int code {  get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
