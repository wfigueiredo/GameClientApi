using GameProducer.Domain.Model;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Validators
{
    public interface IValidator<T>
    {
        Task<T> Validate(T t);
    }
}
