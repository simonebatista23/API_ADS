using ApiUsers.Data;
using ApiUsers.Models;
using ApiUsers.Services;
using Microsoft.EntityFrameworkCore;


namespace ApiUsers.Repositories
{
    public class TicketRepository : ITicket
    {
        private readonly AppDbContext _context;
        private readonly NotificationService _notificationService;

        public TicketRepository(AppDbContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<Tickets>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Tickets> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.TicketsTransctions)
                .FirstOrDefaultAsync(t => t.Id == id);
        }



        public async Task<IEnumerable<Tickets>> GetTicketsOpenedByUserAsync(int userId)
        {
            return await _context.Tickets
                .Include(t => t.TicketsTransctions)
                .Where(t => t.Id_user == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tickets>> GetTicketsRespondedByUserAsync(int userId)
        {
            return await _context.Tickets
                .Include(t => t.TicketsTransctions)
                .Where(t =>
                    t.Id_user != userId && 
                    t.TicketsTransctions.Any(tr =>
                        tr.Id_user_source == userId || tr.Id_user_target == userId)
                )
                .ToListAsync();
        }

        public async Task<IEnumerable<Tickets>> GetTicketsByDepartmentAsync(int deptId)
        {
            return await _context.Tickets
                .Include(t => t.TicketsTransctions)
                .Where(t => t.Id_dept_target == deptId)
                .OrderByDescending(t => t.Open_datetime)
                .ToListAsync();
        }
        public async Task<TicketsTransction?> GetLastTransactionByTicketIdAsync(int ticketId)
        {
            // Usa o IQueryable para consultar a tabela de transações.
            return await _context.TicketsTransctions
                // 1. Filtra as transações apenas para o ticketId fornecido (t.Id_ticket == ticketId)
                .Where(t => t.Id_ticket == ticketId)
                // 2. Ordena os resultados pela data de criação (Created_at) de forma decrescente (o mais novo primeiro)
                .OrderByDescending(t => t.Created_at)
                // 3. Retorna o primeiro item que atende ao critério (a transação mais recente), ou null se não houver nenhuma.
                .FirstOrDefaultAsync();
        }


        public async Task<List<Tickets>> GetAllUserTicketsAsync(int userId)
        {
            return await _context.Tickets
                .Include(t => t.TicketsTransctions)
                .Where(t => t.Id_user == userId) 
                .ToListAsync();
        }

        public async Task<Tickets> CreateAsync(Tickets tickets)
        {
            _context.Tickets.Add(tickets);
            await _context.SaveChangesAsync();

           
            var deptUsers = await _context.Users
                .Where(u => u.IdDept == tickets.Id_dept_target)
                .ToListAsync();

            foreach (var user in deptUsers)
            {
                _notificationService.AddNotification(user.Id, $"Novo chamado: {tickets.Desc}");
            }

            return tickets;
        }


        public async Task UpdateAsync(Tickets tickets)
        {
            _context.Entry(tickets).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Tickets> CreateWithTransactionAsync(Tickets ticket, TicketsTransction transaction)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                transaction.Id_ticket = ticket.Id;
                _context.TicketsTransctions.Add(transaction);
                await _context.SaveChangesAsync();

                // 🔔 Adiciona notificação para os usuários do setor
                var deptUsers = await _context.Users
                    .Where(u => u.IdDept == ticket.Id_dept_target)
                    .ToListAsync();

                foreach (var user in deptUsers)
                {
                    _notificationService.AddNotification(user.Id, $"Novo chamado aberto: {ticket.Desc}");
                }

                // ======= TESTE =======
                //_notificationService.AddNotification(1002, "Teste de notificação");
                //var teste = _notificationService.GetNotifications(1002);
                //Console.WriteLine($"Notifications count for user 1002: {teste.Count}");
                // ======================

                await dbTransaction.CommitAsync();
                return ticket;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var tickets = await _context.Tickets.FindAsync(id);
            if (tickets != null)
            {
                _context.Tickets.Remove(tickets);
                await _context.SaveChangesAsync();
            }
        }
    }
}
