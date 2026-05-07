using Bolillero.Domain;

namespace Bolillero.Tests;

public class BolilleroTests
{
    private readonly Bolillero.Domain.Bolillero _bolillero;

    public BolilleroTests()
    {
        _bolillero = new Bolillero.Domain.Bolillero(10);
        _bolillero.SetAzar(new Primero());
    }

    [Fact]
    public void SacarBolilla_DevuelveCero_YActualizaListas()
    {
        var bolilla = _bolillero.SacarBolilla();

        Assert.Equal(0, bolilla);
        Assert.Equal(9, _bolillero.CantidadBolillasAdentro);
        Assert.Equal(1, _bolillero.CantidadBolillasAfuera);
    }

    [Fact]
    public void ReIngresar_VuelveADejarTodoAdentro()
    {
        _bolillero.SacarBolilla();
        _bolillero.ReingresarBolillas();

        Assert.Equal(10, _bolillero.CantidadBolillasAdentro);
        Assert.Equal(0, _bolillero.CantidadBolillasAfuera);
    }

    [Fact]
    public void JugarGana_ConJugada_0_1_2_3()
    {
        var gano = _bolillero.Jugar(new[] { 0, 1, 2, 3 });

        Assert.True(gano);
    }

    [Fact]
    public void JugarPierde_ConJugada_4_2_1()
    {
        var gano = _bolillero.Jugar(new[] { 4, 2, 1 });

        Assert.False(gano);
    }

    [Fact]
    public void GanarNVeces_ConJugada_0_1_UnaVez_GanaUnaVez()
    {
        var ganadas = _bolillero.JugarNVeces(new[] { 0, 1 }, 1);

        Assert.Equal(1, ganadas);
    }

    [Fact]
    public void Clonar_GeneraUnaCopiaIndependiente()
    {
        _bolillero.SacarBolilla();
        var clon = _bolillero.Clonar();

        _bolillero.SacarBolilla();

        Assert.Equal(8, _bolillero.CantidadBolillasAdentro);
        Assert.Equal(1, clon.CantidadBolillasAfuera);
        Assert.Equal(9, clon.CantidadBolillasAdentro);
    }

    [Fact]
    public void SimularSinHilos_ConAzarDeterministico_DaResultadoEsperado()
    {
        var simulacion = new Simulacion();
        var ganadas = simulacion.SimularSinHilos(_bolillero, Array.Empty<int>(), 100);

        Assert.Equal(100, ganadas);
    }

    [Fact]
    public void SimularConHilos_ConAzarDeterministico_DaResultadoEsperado()
    {
        var simulacion = new Simulacion();
        var ganadas = simulacion.SimularConHilos(_bolillero, Array.Empty<int>(), 100, 4);

        Assert.Equal(100, ganadas);
    }
}

public class Primero : IAzar, IClonable<IAzar>
{
    public int Siguiente(int maximoExclusivo)
    {
        return 0;
    }

    public IAzar Clonar()
    {
        return new Primero();
    }
}
