namespace Bolillero.Domain;

public class Simulacion
{
    public long SimularSinHilos(Bolillero bolillero, IReadOnlyList<int> jugada, int cantidadSimulaciones)
    {
        if (bolillero is null)
        {
            throw new ArgumentNullException(nameof(bolillero));
        }

        if (jugada is null)
        {
            throw new ArgumentNullException(nameof(jugada));
        }

        if (cantidadSimulaciones < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cantidadSimulaciones), "Debe ser mayor o igual a 0.");
        }

        var copiaBolillero = bolillero.Clonar();
        return copiaBolillero.JugarNVeces(jugada, cantidadSimulaciones);
    }

    public long SimularConHilos(Bolillero bolillero, IReadOnlyList<int> jugada, int cantidadSimulaciones, int cantidadHilos)
    {
        if (bolillero is null)
        {
            throw new ArgumentNullException(nameof(bolillero));
        }

        if (jugada is null)
        {
            throw new ArgumentNullException(nameof(jugada));
        }

        if (cantidadSimulaciones < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cantidadSimulaciones), "Debe ser mayor o igual a 0.");
        }

        if (cantidadHilos <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cantidadHilos), "Debe ser mayor a 0.");
        }

        long ganadas = 0;
        var tareas = new List<Task>(cantidadHilos);
        var basePorHilo = cantidadSimulaciones / cantidadHilos;
        var resto = cantidadSimulaciones % cantidadHilos;

        for (var i = 0; i < cantidadHilos; i++)
        {
            var simulacionesEnHilo = basePorHilo + (i < resto ? 1 : 0);

            if (simulacionesEnHilo == 0)
            {
                continue;
            }

            tareas.Add(Task.Run(() =>
            {
                var copiaBolillero = bolillero.Clonar();
                var ganadasParciales = copiaBolillero.JugarNVeces(jugada, simulacionesEnHilo);
                Interlocked.Add(ref ganadas, ganadasParciales);
            }));
        }

        Task.WaitAll(tareas.ToArray());
        return ganadas;
    }
}
