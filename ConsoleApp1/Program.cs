/*
 CÓDIGO 1
 */
/*
using System;
using System.Collections.Generic;

namespace MemoriaParticoesVariaveis
{
    class Program
    {
        static int tamanhoMaximoMemoria; // Tamanho máximo da memória física em KB
        static List<Memoria> memoriaFisica = new List<Memoria>(); // Lista para representar a memória física
        static List<Processo> processos = new List<Processo>(); // Lista para armazenar os processos

        // Classe para representar a memória (partição)
        class Memoria
        {
            public bool Livre { get; set; }
            public int Tamanho { get; set; }
            public Processo Processo { get; set; }
        }

        // Classe para representar um processo
        class Processo
        {
            public string Nome { get; set; }
            public int Id { get; set; }
            public int Tamanho { get; set; }
        }

        // Função para alocar memória para um novo processo usando first-fit
        static bool AlocarFirstFit(string nome, int idProcesso, int tamanhoProcesso)
        {
            for (int i = 0; i < memoriaFisica.Count; i++)
            {
                if (memoriaFisica[i].Livre && memoriaFisica[i].Tamanho >= tamanhoProcesso)
                {
                    memoriaFisica[i].Livre = false;
                    memoriaFisica[i].Processo = new Processo
                    {
                        Nome = nome,
                        Id = idProcesso,
                        Tamanho = tamanhoProcesso
                    };
                    return true;
                }
            }
            return false;
        }

        // Função para criar e adicionar um novo processo
        static void CriarProcesso(string nome, int idProcesso, int tamanhoProcesso)
        {
            Processo processo = new Processo
            {
                Nome = nome,
                Id = idProcesso,
                Tamanho = tamanhoProcesso
            };
            processos.Add(processo);
        }

        // Função para liberar memória de um processo
        static bool LiberarMemoria(int idProcesso)
        {
            for (int i = 0; i < memoriaFisica.Count; i++)
            {
                if (!memoriaFisica[i].Livre && memoriaFisica[i].Processo.Id == idProcesso)
                {
                    memoriaFisica[i].Livre = true;
                    memoriaFisica[i].Processo = null;
                    return true;
                }
            }
            return false;
        }

        // Função para mostrar o estado da memória física
        static void MostrarEstadoMemoria()
        {
            int memoriaLivre = 0;
            int memoriaAlocada = 0;
            int fragmentacaoExterna = 0;

            Console.WriteLine("Estado da memória:");
            for (int i = 0; i < memoriaFisica.Count; i++)
            {
                if (memoriaFisica[i].Livre)
                {
                    Console.WriteLine($"Partição {i + 1}: Livre - {memoriaFisica[i].Tamanho} KB");
                    memoriaLivre += memoriaFisica[i].Tamanho;
                    fragmentacaoExterna += memoriaFisica[i].Tamanho;
                }
                else
                {
                    Console.WriteLine($"Partição {i + 1}: {memoriaFisica[i].Processo.Nome} - {memoriaFisica[i].Tamanho} KB");
                    memoriaAlocada += memoriaFisica[i].Tamanho;
                }
            }

            int memoriaTotal = memoriaLivre + memoriaAlocada;
            int fragmentacaoInterna = 0;
            if (memoriaAlocada > 0)
            {
                fragmentacaoInterna = memoriaTotal - memoriaAlocada;
                fragmentacaoExterna -= memoriaTotal - memoriaFisica[0].Tamanho;
            }

            Console.WriteLine($"Capacidade total: {memoriaTotal} KB");
            Console.WriteLine($"Memória livre: {memoriaLivre} KB");
            Console.WriteLine($"Memória alocada: {memoriaAlocada} KB");
            Console.WriteLine($"Fragmentação interna: {fragmentacaoInterna} KB");
            Console.WriteLine($"Fragmentação externa: {fragmentacaoExterna} KB");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Digite o tamanho máximo da memória física em KB:");
            tamanhoMaximoMemoria = int.Parse(Console.ReadLine());

            // Inicializa a memória física com uma única partição de tamanho máximo
            memoriaFisica.Add(new Memoria { Livre = true, Tamanho = tamanhoMaximoMemoria });

            // Exemplo de uso:
            CriarProcesso("Processo A", 1, 200);
            CriarProcesso("Processo B", 2, 150);
            CriarProcesso("Processo C", 3, 300);
            CriarProcesso("Processo D", 4, 250);

            // Aloca memória usando first-fit para cada processo
            foreach (Processo processo in processos)
            {
                bool alocado = AlocarFirstFit(processo.Nome, processo.Id, processo.Tamanho);
                if (!alocado)
                {
                    Console.WriteLine($"Não há espaço suficiente para alocar o processo '{processo.Nome}' com tamanho {processo.Tamanho} KB.");
                }
            }

            // Mostra o estado da memória após a alocação
            MostrarEstadoMemoria();
        }
    }
}
*/

/////////////////////////////////////////////////////////////////////////////////////////

/*
CÓDIGO 2
*/

using System;
using System.Collections.Generic;

namespace MemoriaPaginacao
{
    class Program
    {
        static int tamanhoMaximoMemoriaFisica; // Tamanho máximo da memória física em KB
        static int tamanhoMaximoMemoriaVirtual; // Tamanho máximo da memória virtual em KB
        static int tamanhoPagina; // Tamanho das páginas em KB

        // Classe para representar a memória física (partição)
        class MemoriaFisica
        {
            public bool Livre { get; set; }
            public int Pagina { get; set; }
        }

        // Classe para representar um processo
        class Processo
        {
            public string Nome { get; set; }
            public int Id { get; set; }
            public int Tamanho { get; set; }
            public int Paginas { get; set; }
        }

        static List<MemoriaFisica> memoriaFisica = new List<MemoriaFisica>();
        static List<Processo> processos = new List<Processo>();

        // Função para inicializar a memória física com as páginas disponíveis
        static void InicializarMemoriaFisica()
        {
            int numPaginas = tamanhoMaximoMemoriaFisica / tamanhoPagina;
            for (int i = 0; i < numPaginas; i++)
            {
                memoriaFisica.Add(new MemoriaFisica { Livre = true, Pagina = -1 });
            }
        }

        // Função para criar e adicionar um novo processo
        static void CriarProcesso(string nome, int idProcesso, int tamanhoProcesso)
        {
            Processo processo = new Processo
            {
                Nome = nome,
                Id = idProcesso,
                Tamanho = tamanhoProcesso,
                Paginas = (int)Math.Ceiling((double)tamanhoProcesso / tamanhoPagina)
            };
            processos.Add(processo);
        }

        // Função para alocar memória para um novo processo usando o algoritmo de substituição FIFO
        static void AlocarPaginasFIFO(Processo processo)
        {
            int paginasAlocadas = 0;
            for (int i = 0; i < memoriaFisica.Count && paginasAlocadas < processo.Paginas; i++)
            {
                if (memoriaFisica[i].Livre)
                {
                    memoriaFisica[i].Livre = false;
                    memoriaFisica[i].Pagina = processo.Id;
                    paginasAlocadas++;
                }
            }
        }

        // Função para mostrar o estado da memória física
        static void MostrarEstadoMemoria()
        {
            int memoriaAlocada = 0;
            int paginasLivre = 0;
            int paginasAlocadas = 0;

            Console.WriteLine("Estado da memória:");
            for (int i = 0; i < memoriaFisica.Count; i++)
            {
                if (memoriaFisica[i].Livre)
                {
                    paginasLivre++;
                }
                else
                {
                    paginasAlocadas++;
                    memoriaAlocada += tamanhoPagina;
                    Console.WriteLine($"Página {i + 1}: Processo {memoriaFisica[i].Pagina} - {tamanhoPagina} KB");
                }
            }

            Console.WriteLine($"Capacidade total da memória física: {tamanhoMaximoMemoriaFisica} KB");
            Console.WriteLine($"Memória alocada: {memoriaAlocada} KB");
            Console.WriteLine($"Páginas livres: {paginasLivre}");
            Console.WriteLine($"Páginas alocadas: {paginasAlocadas}");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Digite o tamanho máximo da memória física em KB:");
            tamanhoMaximoMemoriaFisica = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite o tamanho máximo da memória virtual em KB:");
            tamanhoMaximoMemoriaVirtual = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite o tamanho das páginas em KB:");
            tamanhoPagina = int.Parse(Console.ReadLine());

            InicializarMemoriaFisica();

            // Exemplo de uso:
            CriarProcesso("Processo A", 1, 400);
            CriarProcesso("Processo B", 2, 600);
            CriarProcesso("Processo C", 3, 300);

            // Aloca memória para cada processo usando o algoritmo de substituição FIFO
            foreach (Processo processo in processos)
            {
                AlocarPaginasFIFO(processo);
            }

            // Mostra o estado da memória após a alocação
            MostrarEstadoMemoria();
        }
    }
}
