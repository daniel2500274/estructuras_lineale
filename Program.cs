using Playlist_circular_con_prev_next;
using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        var playlist = new Playlist();
        playlist.AddLast(new Song("Animals", "Martin Garrix", 186));
        playlist.AddLast(new Song("In the Name of Love", "Martin Garrix & Bebe Rexha", 195));
        playlist.AddLast(new Song("Scared to Be Lonely", "Martin Garrix & Dua Lipa", 220));
        playlist.AddLast(new Song("High On Life", "Martin Garrix ft. Bonn", 230));
        playlist.AddLast(new Song("Wake Me Up", "Avicii", 247));
        playlist.AddLast(new Song("Clarity", "Zedd ft. Foxes", 271));
        playlist.AddLast(new Song("Don't You Worry Child", "Swedish House Mafia", 299));
        playlist.AddLast(new Song("This Is What You Came For", "Calvin Harris ft. Rihanna", 222));
        playlist.AddLast(new Song("Titanium", "David Guetta ft. Sia", 245));
        playlist.AddLast(new Song("Summer Days", "Martin Garrix ft. Macklemore", 163));

        int option;
        do
        {
            Console.Clear();
            DisplayHeader();
            DisplayMenu();

            if (!int.TryParse(Console.ReadLine(), out option))
            {
                option = 0;
            }

            switch (option)
            {
                case 1:
                    AddSongMenu(playlist);
                    break;

                case 2:
                    RemoveSongMenu(playlist);
                    break;

                case 3:
                    PrintTitles(playlist);
                    break;

                case 4:
                    NextSong(playlist);
                    break;

                case 5:
                    PrevSong(playlist);
                    break;

                case 6:
                    ShuffleMenu(playlist);
                    break;

                case 7:
                    ExportJson(playlist);
                    break;

                case 8:
                    DisplayExitMessage();
                    break;

                default:
                    DisplayError("Opción inválida. Por favor, seleccione una opción entre 1 y 8.");
                    break;
            }

            if (option != 8)
            {
                Console.WriteLine("\nPresione ENTER para continuar...");
                Console.ReadLine();
            }

        } while (option != 8);
    }

    // ═══════════════════════════════════════════════════════════════
    // MÉTODOS DE PRESENTACIÓN
    // ═══════════════════════════════════════════════════════════════

    static void DisplayHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                   REPRODUCTOR DE MÚSICA                       ║"); 
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    static void DisplayMenu()
    {
        Console.WriteLine("  [1] Agregar canción");
        Console.WriteLine("  [2] Eliminar canción");
        Console.WriteLine("  [3] Ver playlist completa");
        Console.WriteLine("  [4] Reproducir siguiente");
        Console.WriteLine("  [5] Reproducir anterior");
        Console.WriteLine("  [6] Mezclar playlist (Shuffle)");
        Console.WriteLine("  [7] Exportar a JSON");
        Console.WriteLine("  [8] Salir");
        Console.WriteLine();
        Console.Write("  Opción: ");
    }

    static void DisplaySectionHeader(string title)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"─── {title} " + new string('─', 50 - title.Length));
        Console.ResetColor();
        Console.WriteLine();
    }

    static void DisplaySuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✓ {message}");
        Console.ResetColor();
    }

    static void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n✗ {message}");
        Console.ResetColor();
    }

    static void DisplayInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"  {message}");
        Console.ResetColor();
    }

    static void DisplayExitMessage()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                Gracias por usar Music Player                  ║");
        Console.WriteLine("║                      ¡Hasta pronto!                           ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
    }

    // ═══════════════════════════════════════════════════════════════
    // MÉTODOS DE ENTRADA DE DATOS
    // ═══════════════════════════════════════════════════════════════

    static string ReadString(string prompt)
    {
        string input;
        while (true)
        {
            Console.Write($"  {prompt}: ");
            input = Console.ReadLine()!;
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            DisplayError("El campo no puede estar vacío. Intente nuevamente.");
        }
    }

    static int ReadInt(string prompt)
    {
        int value;
        while (true)
        {
            Console.Write($"  {prompt}: ");
            if (int.TryParse(Console.ReadLine(), out value))
            {
                return value;
            }
            DisplayError("Entrada inválida. Por favor, ingrese un número válido.");
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // MÉTODOS DEL MENÚ
    // ═══════════════════════════════════════════════════════════════

    static void AddSongMenu(Playlist pl)
    {
        DisplaySectionHeader("AGREGAR CANCIÓN");

        string title = ReadString("Título");
        string artist = ReadString("Artista");
        int duration = ReadInt("Duración (segundos)");

        var song = new Song(title, artist, duration);
        pl.AddLast(song);

        DisplaySuccess("Canción agregada exitosamente a la playlist.");
    }

    static void RemoveSongMenu(Playlist pl)
    {
        DisplaySectionHeader("ELIMINAR CANCIÓN");
        PrintTitles(pl);

        if (pl.Count == 0)
        {
            return;
        }

        Guid id;
        while (true)
        {
            Console.Write("\n  ID de la canción a eliminar (o 'c' para cancelar): ");
            string input = Console.ReadLine()!;

            if (input.ToLower() == "c")
            {
                DisplayInfo("Operación cancelada.");
                return;
            }

            if (Guid.TryParse(input, out id))
            {
                break;
            }
            DisplayError("Formato de ID inválido. Por favor, copie y pegue el ID desde la lista.");
        }

        bool removed = pl.RemoveById(id);

        if (removed)
        {
            DisplaySuccess("Canción eliminada correctamente de la playlist.");
        }
        else
        {
            DisplayError("No se encontró ninguna canción con el ID especificado.");
        }
    }

    static void ShuffleMenu(Playlist pl)
    {
        DisplaySectionHeader("MEZCLAR PLAYLIST");
        int seed = ReadInt("Número de semilla para aleatorización");

        pl.Shuffle(seed);
        DisplaySuccess("Playlist mezclada correctamente.");
    }

    static void ExportJson(Playlist pl)
    {
        DisplaySectionHeader("EXPORTAR A JSON");
        string json = pl.ExportTitlesJson();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(json);
        Console.ResetColor();

        DisplaySuccess("Datos exportados en formato JSON.");
    }

    static void NextSong(Playlist pl)
    {
        try
        {
            var s = pl.Next();
            DisplaySectionHeader("REPRODUCIENDO");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ▶ {s.Title}");
            Console.WriteLine($"    {s.Artist}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            DisplayError(ex.Message);
        }
    }

    static void PrevSong(Playlist pl)
    {
        try
        {
            var s = pl.Prev();
            DisplaySectionHeader("REPRODUCIENDO");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ◀ {s.Title}");
            Console.WriteLine($"    {s.Artist}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            DisplayError(ex.Message);
        }
    }

    static void PrintTitles(Playlist pl)
    {
        DisplaySectionHeader("PLAYLIST ACTUAL");

        if (pl.Count == 0)
        {
            DisplayInfo("La playlist está vacía.");
            return;
        }

        Console.WriteLine($"  Total de canciones: {pl.Count}");
        Console.WriteLine();

        int i = 1;
        foreach (var s in pl)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  {i,2}. ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{s.Title}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"      Artista: {s.Artist} | Duración: {s.DurationInSeconds}s");
            Console.WriteLine($"      ID: {s.Id}");
            Console.ResetColor();
            i++;
        }

        try
        {
            var currentSong = pl.Current();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ┌─────────────────────────────────────────────────────────────┐");
            Console.WriteLine("  │ REPRODUCIENDO AHORA                                         │");
            Console.WriteLine("  ├─────────────────────────────────────────────────────────────┤");
            Console.Write("  │ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{currentSong.Title,-58}  │");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  │ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{currentSong.Artist,-58}  │");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  └─────────────────────────────────────────────────────────────┘");
            Console.ResetColor();
        }
        catch (Exception)
        {
            Console.WriteLine();
            DisplayInfo("No hay ninguna canción seleccionada actualmente.");
        }
    }
}