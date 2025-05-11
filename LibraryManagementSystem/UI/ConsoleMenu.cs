using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.ConsoleApp.Service;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace LibraryManagementSystem.ConsoleApp.UI
{
    public class ConsoleMenu
    {
        public bool running = true;
        private Service.Service service;

        private Dictionary<int, Action> options;
        public ConsoleMenu(Service.Service serv) {
            service = serv;
            options = new Dictionary<int, Action>
            {
                {0, () => running = false },
                {1, PrintMenu },
                {2, AddBook },
                {3, UpdateBook },
                {4, DeleteBook },
                {5, SearchBook },
                {6, AddBookLoanMenu },
                {7, ReturnBookMenu },
                {8, PrintActiveLoans },
                {9, PrintStatistics }

            };
        }

        public void ShowMainMenu()
        {
            Console.WriteLine("\n---Library Management System---\n");
            Console.WriteLine("1. Print Books/Authors");
            Console.WriteLine("2. Add Book");
            Console.WriteLine("3. Update Book");
            Console.WriteLine("4. Delete Book");
            Console.WriteLine("5. Search Books/Authors");
            Console.WriteLine("6. Add Book Loan");
            Console.WriteLine("7. Return Book");
            Console.WriteLine("8. Print Active Loans");
            Console.WriteLine("9. Statistics");
            Console.WriteLine("0. Exit\n");
            
            ProcessInput();
        }

        private void ProcessInput()
        {
            Console.Write("Enter Option: ");
            try
            {
                int option = Convert.ToInt32(Console.ReadLine());
                options[option]();

            } catch (System.FormatException)
            {
                Console.WriteLine("Invalid Input");
            }
        }
        private void PrintMenu()
        {
            ShowOptionBookOrAuthor();
            try
            {
                int option = Convert.ToInt32(Console.ReadLine());
                if (option == 1)
                {
                    List<string> books = service.GetAllBooks();
                    for(int i = 0; i < books.Count; i++)
                    {
                        Console.WriteLine(books[i]);
                    }
                }
                if (option == 2)
                {
                    List<string> authors = service.GetAllAuthors();
                    for(int i = 0; i < authors.Count; i++)
                    {
                        Console.WriteLine(authors[i]);
                    }
                }
            } catch (System.FormatException)
            {
                Console.WriteLine("Invalid Input");
            }
        }

        private void PrintBooksOrAuthors()
        {
            ShowOptionBookOrAuthor();
        }

        private void AddBookLoanMenu()
        {
            try
            {
                Console.Write("\nEnter Book ID: ");
                string id_str = Console.ReadLine();
                if (Guid.TryParse(id_str, out Guid id))
                {
                    Console.Write("\nEnter Borrower Name: ");
                    string personName = Console.ReadLine();
                    DateTime borrowDate = DateTime.Now;
                    Console.Write("\nEnter Borrow Time (in days): ");
                    int days = Convert.ToInt32(Console.ReadLine());
                    DateTime returnDate = borrowDate.AddDays(days);

                    service.AddBookLoan(id, personName, borrowDate, returnDate);
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            } catch (System.FormatException)
            {
                Console.WriteLine("Invalid Input");
            }
            
        }

        private void ReturnBookMenu()
        {
            Console.WriteLine("\nEnter Loan ID: ");
            string id_str = Console.ReadLine();
            if(Guid.TryParse(id_str , out Guid id))
            {
                service.CancelLoan(id);
            }
            else
            {
                Console.WriteLine("Invalid Input");
            }
        }

        private void PrintStatistics()
        {
            List<string> mostRequested = service.GetMostRequestedBooks();
            Console.WriteLine("\nMost Loaned Books: ");
            foreach(string s  in mostRequested)
            {
                Console.WriteLine(s);
            }
            List<string> leastRequested = service.GetLeastRequestedBooks();
            Console.WriteLine("\nLeast Loaned Books: ");
            foreach(string s in leastRequested)
            {
                Console.WriteLine(s);
            }
        }
        private void ShowOptionBookOrAuthor()
        {
            Console.WriteLine("\nChoose: ");
            Console.WriteLine("1. Books");
            Console.WriteLine("2. Authors\n");
            Console.Write("Enter Option: ");

        }
        private void AddBook()
        {
            Console.Write("Enter Book Title: ");
            string title = Console.ReadLine();
            List<string> authorNames = new List<string>();
            Console.Write("\nEnter Author Count: ");
            try
            {
                int authorCount = Convert.ToInt32(Console.ReadLine());
                for(int i = 1; i<= authorCount; i++)
                {
                    Console.Write("\nEnter Author: ");
                    string author = Console.ReadLine();
                    authorNames.Add(author);
                }

                Console.Write("\nEnter Description: ");
                string description = Console.ReadLine();
                Console.Write("\nEnter Quantity: ");
                int quantity = Convert.ToInt32(Console.ReadLine());
                Console.Write("\nEnter Borrow Price: ");
                int borrowPrice = Convert.ToInt32(Console.ReadLine());

                service.AddBook(title, quantity, description, borrowPrice, authorNames);

            } catch (System.FormatException)
            {
                Console.WriteLine("\nInvalid Input");
            }
        }

        private void UpdateBook()
        {
            try
            {
                Console.Write("\nEnter Book ID: ");
                string id_str = Console.ReadLine();

                if (Guid.TryParse(id_str, out Guid id))
                {
                    Console.Write("\nEnter New Title: ");
                    string title = Console.ReadLine();
                    Console.Write("\nEnter New Description: ");
                    string description = Console.ReadLine();
                    Console.Write("\nEnter New Borrow Price: ");
                    int borrowPrice = Convert.ToInt32(Console.ReadLine());
                    Console.Write("\nEnter New Quantity: ");
                    int quantity = Convert.ToInt32(Console.ReadLine());
                    Console.Write("\nEnter Count of Authors: ");
                    int authorCount = Convert.ToInt32(Console.ReadLine());
                    List<Guid> authorIds = new List<Guid>();
                    for (int i = 1; i <= authorCount; i++)
                    {
                        Console.Write("\nEnter Author ID: ");
                        string id_str_author = Console.ReadLine();
                        if (Guid.TryParse(id_str_author, out Guid id_author)) { authorIds.Add(id_author); }
                        else { Console.WriteLine("Invalid Input"); }
                    }
                    service.UpdateBook(id, title, description, borrowPrice, quantity, authorIds);
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            } catch (System.FormatException)
            {
                Console.WriteLine("Invalid Input");
            }
            
        }

        private void DeleteBook()
        {
            Console.Write("\nEnter Book ID: ");
            string id_str = Console.ReadLine();

            if(Guid.TryParse(id_str, out Guid id))
            {
                service.DeleteBook(id);
            }
            else
            {
                Console.WriteLine("Invalid Input");
            }
            
        }

        private void SearchBook()
        {
            try
            {

                Console.WriteLine("\n1. Search By Title");
                Console.WriteLine("2. Search By Author");
                Console.Write("\nEnter Option: ");
                int searchOption = Convert.ToInt32(Console.ReadLine());
                if(searchOption == 1)
                {
                    Console.Write("\nEnter Title: ");
                    string title = Console.ReadLine();
                    List<string> results = service.SearchBookByTitle(title);
                    for(int i =0; i < results.Count; i++)
                    {
                        Console.WriteLine(results[i]);
                    }
                }
                if(searchOption == 2)
                {
                    Console.Write("\nEnter Author: ");
                    string author = Console.ReadLine();
                    List<string> results = service.SearchBookByAuthor(author);
                    for(int i=0; i< results.Count; i++) { Console.WriteLine(results[i]); }
                }
            } catch (System.FormatException)
            {
                Console.WriteLine("Invalid Input");
            }
            Console.WriteLine();
        }

        private void PrintActiveLoans()
        {
            List<string> found = service.GetAllLoans();
            foreach(string item in found)
            {
                Console.WriteLine(item);
            }
        }
    }
}
