using GAPPLE.Shared.Helpers;

namespace GAPPLE.Shared.Entities
{
    public class Estado<T>
    {
        [ColumnName("IdEstado")]
        public T Id { get; set; }

        [ColumnName("Descripcion")]
        public string Descripcion { get; set; }

        public Estado() { }
        public Estado(T id, string descripcion) => (Id, Descripcion) = (id, descripcion);
    }
}
