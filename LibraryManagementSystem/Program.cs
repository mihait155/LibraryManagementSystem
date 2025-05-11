using Library.Infrastructure;
using Library.Infrastructure.Repositories;
using LibraryManagementSystem.ConsoleApp.Service;
using LibraryManagementSystem.ConsoleApp.UI;

var context = new LibraryDBContext();

AuthorRepository authorRepository = new AuthorRepository(context);
BookRepository bookRepository = new BookRepository(context);
LoanRepository loanRepository = new LoanRepository(context);
Service service = new Service(bookRepository, authorRepository, loanRepository);

ConsoleMenu consoleMenu = new ConsoleMenu(service);

while (consoleMenu.running)
{
    consoleMenu.ShowMainMenu();
}