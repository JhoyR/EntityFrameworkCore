using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new Data.ApplicationContext();

            //Não indicado para uso em produção
            // db.Database.Migrate();

            var existe = db.Database.GetPendingMigrations().Any();
            if (existe)
            {
                //Regra de negocio a ser adicionada
            }
            // InserirDados();
            // InserirDadosEmMassa();
            // ConsultarDados();
            // CadastrarPedidos();
            // AtualizarDados();
            RemoverRegistro();
        }
        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            // var cliente = db.Clientes.Find(3); //Primeiro localiza o registro, depois deleta
            var cliente = new Cliente {Id = 3}; //Apenas uma interação

            // db.Clientes.Remove(cliente);
            // db.Remove(cliente);
            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }
        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            // var cliente = db.Clientes.FirstOrDefault(p=>p.Id == 1);
            // var cliente = db.Clientes.Find(1);

            var cliente = new Cliente
            {
                Id = 1
            };

            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado Passo 3",
                Telefone = "93999993333"
            };

            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            // db.Entry(cliente).State = EntityState.Modified; //Informa de forma explícita para atualizar todas as propriedades da instância
            // db.Clientes.Update(cliente); //Atualiza tudo, sobrescreve todas as propriedades
            db.SaveChanges();
        }
        private static void CadastrarPedidos()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Items = new List<PedidoItem>{
                    new PedidoItem {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                }
            }
            };
            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }
        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db
            .Pedidos
            .Include(p => p.Items)
            .ThenInclude(p => p.Produto) //dentro do include
            .ToList();

            Console.WriteLine(pedidos.Count);
        }
        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            // var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var consultaPorMetodoLinq = db.Clientes.AsNoTracking()
                 .Where(p => p.Id > 0)
                 .OrderBy(p => p.Id)
                 .ToList();

            foreach (var cliente in consultaPorMetodoLinq)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");
                // db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }
        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste 2",
                CodigoBarras = "1234567891232",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };
            var cliente = new Cliente
            {
                Nome = "Jhoy Rodrigues",
                CEP = "99999000",
                Cidade = "Cariacica",
                Estado = "ES",
                Telefone = "22999994444"
            };
            using var db = new Data.ApplicationContext();
            db.AddRange(produto, cliente);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }
        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };
            using var db = new Data.ApplicationContext();
            // db.Produtos.Add(produto);
            // db.Set<Produto>().Add(produto);
            // db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }
    }
}