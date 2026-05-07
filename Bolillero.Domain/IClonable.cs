namespace Bolillero.Domain;

public interface IClonable<out T>
{
    T Clonar();
}
