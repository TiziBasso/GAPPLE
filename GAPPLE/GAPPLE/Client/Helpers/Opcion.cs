namespace GAPPLE.Client.Helpers
{
    public class Opcion
    {
        public object Id { get; set; }
        public string Descripcion { get; set; }
        public bool Check { get; set; }

        public Opcion(object id, string descripcion) => (Id, Descripcion) = (id, descripcion);

        public Opcion(char id, string descripcion) => (Id, Descripcion) = (id, descripcion);

        public Opcion(string id, string descripcion) => (Id, Descripcion) = (id, descripcion);

        public Opcion(int id, string descripcion, bool check = false) => (Id, Descripcion, Check) = (id, descripcion, check);

        public Opcion(int id, string descripcion) => (Id, Descripcion) = (id, descripcion);

        public Opcion() { }
    }
}
