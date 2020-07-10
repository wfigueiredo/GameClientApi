using GameClientApi.Domain.Model;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Validators
{
    public interface IValidator<T>
    {
        Task<T> Validate(T t);
    }
}
