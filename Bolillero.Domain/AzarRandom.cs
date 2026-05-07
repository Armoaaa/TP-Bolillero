namespace Bolillero.Domain;

public class AzarRandom : IAzar, IClonable<IAzar>
{
    private readonly Random _random = new();

    public int Siguiente(int maximoExclusivo)
    {
        return _random.Next(maximoExclusivo);
    }

    public IAzar Clonar()
    {
        return new AzarRandom();
    }
}
