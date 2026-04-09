namespace Bolillero.Domain;

public class Bolillero
{
    private readonly List<int> _bolillasAdentro;
    private readonly List<int> _bolillasAfuera;
    private IAzar _azar;

    public Bolillero(int cantidadBolillas, IAzar? azar = null)
    {
        if (cantidadBolillas <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cantidadBolillas), "La cantidad debe ser mayor a 0.");
        }

        _bolillasAdentro = Enumerable.Range(0, cantidadBolillas).ToList();
        _bolillasAfuera = new List<int>();
        _azar = azar ?? new AzarRandom();
    }

    public int CantidadBolillasAdentro => _bolillasAdentro.Count;
    public int CantidadBolillasAfuera => _bolillasAfuera.Count;

    public void SetAzar(IAzar azar)
    {
        _azar = azar ?? throw new ArgumentNullException(nameof(azar));
    }

    public int SacarBolilla()
    {
        if (_bolillasAdentro.Count == 0)
        {
            throw new InvalidOperationException("No quedan bolillas dentro del bolillero.");
        }

        var indice = _azar.Siguiente(_bolillasAdentro.Count);
        var bolilla = _bolillasAdentro[indice];
        _bolillasAdentro.RemoveAt(indice);
        _bolillasAfuera.Add(bolilla);

        return bolilla;
    }

    public bool Jugar(IReadOnlyList<int> jugada)
    {
        if (jugada is null)
        {
            throw new ArgumentNullException(nameof(jugada));
        }

        if (jugada.Count == 0)
        {
            return true;
        }

        if (jugada.Count > _bolillasAdentro.Count)
        {
            return false;
        }

        for (var i = 0; i < jugada.Count; i++)
        {
            if (SacarBolilla() != jugada[i])
            {
                return false;
            }
        }

        return true;
    }

    public int JugarNVeces(IReadOnlyList<int> jugada, int cantidadVeces)
    {
        if (cantidadVeces < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cantidadVeces), "Debe ser mayor o igual a 0.");
        }

        var ganadas = 0;
        for (var i = 0; i < cantidadVeces; i++)
        {
            if (Jugar(jugada))
            {
                ganadas++;
            }

            ReingresarBolillas();
        }

        return ganadas;
    }

    public Task<int> JugarNVecesAsync(IReadOnlyList<int> jugada, int cantidadVeces)
    {
        return Task.FromResult(JugarNVeces(jugada, cantidadVeces));
    }

    public void ReingresarBolillas()
    {
        _bolillasAdentro.AddRange(_bolillasAfuera);
        _bolillasAfuera.Clear();
    }
}
