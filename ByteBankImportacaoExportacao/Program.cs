using System;
using System.IO;
using System.Text;
using ByteBankImportacaoExportacao.Modelos;

namespace ByteBankImportacaoExportacao
{
    class Program
    {
        static void Main(string[] args)
        {
            UsandoStreamDeEntradaDaConsole();
            Console.ReadLine();
        }

        static void UsandoStreamDeEntradaDaConsole()
        {
            using (var fluxoDeEntrada = Console.OpenStandardInput())
            using (var fs = new FileStream("entradaConsole.txt", FileMode.Create))
            {
                var buffer = new byte[1024];
                
                while (true)
                {
                    var bytesLidos = fluxoDeEntrada.Read(buffer, 0, 1024);
                    fs.Write(buffer, 0, bytesLidos);
                    fs.Flush();
                    System.Console.WriteLine($"Bytes lidos da console {bytesLidos}");
                }
            }
        }

        static void StreamBinario()
        {
            using (var fs = new FileStream("ContaCorrente.txt", FileMode.Create))
            using (var escritor = new BinaryWriter(fs))
            {
                escritor.Write(456);
                escritor.Write(123456);
                escritor.Write(8764.90);
                escritor.Write("Gustavo Braga");
            }
        }

        static void LeituraBinaria()
        {
            using (var fs = new FileStream("ContaCorrente.txt", FileMode.Open))
            using (var leitor = new BinaryReader(fs))
            {
                var agencia = leitor.ReadInt32();
                var numeroConta = leitor.ReadInt32();
                var saldo = leitor.ReadDouble();
                var titular = leitor.ReadString();

                System.Console.WriteLine($"{agencia}/{numeroConta}, {saldo}, {titular}");
            }
        }

        static void CriarArquivoComWriter()
        {
            var caminhoNovoArquivo = "exportContas.csv";

            using (var fluxoDeArquivo = new FileStream(caminhoNovoArquivo, FileMode.Create))
            using (var escritor = new StreamWriter(fluxoDeArquivo, Encoding.Default))
            {
                escritor.Write("456, 12345, 5467.50, Pedro Teste");
            }
        }

        static void CriarArquivo()
        {
            var caminhoNovoArquivo = "exportContas.csv";

            using (var fluxoDeArquivo = new FileStream(caminhoNovoArquivo, FileMode.Create))
            {
                var contaComoString = "456, 12345, 5467.50, Gustavo Santos";
                
                var encoding = Encoding.UTF8;
                var bytes = encoding.GetBytes(contaComoString);
                fluxoDeArquivo.Write(bytes, 0, contaComoString.Length);
            }
        }

        static void ImportandoContas()
        {
            var enderecoDoArquivo = "contas.txt";

            using (var fluxoDeArquivo = new FileStream(enderecoDoArquivo, FileMode.Open))
            using (var leitor = new StreamReader(fluxoDeArquivo))
            {
                while (!leitor.EndOfStream)
                {
                    var linha = leitor.ReadLine();
                    var contaCorrente = ConverterStringParaContaCorrente(linha);
                    System.Console.WriteLine($"{contaCorrente.Titular.Nome}, Conta Número: {contaCorrente.Numero}, ag. {contaCorrente.Agencia}, saldo: {contaCorrente.Saldo}");
                }
            }
        }

        static ContaCorrente ConverterStringParaContaCorrente(string linha)
        {
            string[] campos = linha.Split(',');
            
            var agencia = campos[0];
            var numero = campos[1];
            var saldo = campos[2];
            var nomeTitular = campos[3];

            var intAgencia = int.Parse(agencia);
            var intNumero = int.Parse(numero);
            var doubleSaldo = double.Parse(saldo);

            var titular = new Clientes();
            titular.Nome = nomeTitular;

            var resultado = new ContaCorrente(intAgencia, intNumero);
            resultado.Depositar(doubleSaldo);
            resultado.Titular = titular;

            return resultado;
        }

        static void LidandoComFileStreamDiretamente()
        {
            var enderecoDoArquivo = "contas.txt";

            using (var fluxoDoArquivo = new FileStream(enderecoDoArquivo, FileMode.Open))
            {
                var buffer = new byte[1024];
                var numeroDeBytesLidos = -1;

                while (numeroDeBytesLidos != 0)
                {
                    numeroDeBytesLidos = fluxoDoArquivo.Read(buffer, 0, 1024);
                    EscreverBuffer(buffer, numeroDeBytesLidos);
                }
            }
        }
        
        static void EscreverBuffer(byte[] buffer, int bytesLidos)
        {
            var utf8 = Encoding.Default; //new UTF8Encoding();
            var texto = utf8.GetString(buffer, 0, bytesLidos);

            Console.Write(texto);

            // foreach (var meuByte in buffer)
            // {
            //     Console.Write(meuByte);
            //     Console.Write(" ");
            // }
        }
    }
}
