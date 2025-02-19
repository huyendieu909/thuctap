using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static System.Console;
namespace NCTR.M.A06
{
    internal class Program
    {
        public static BookInventory bookInventory = new BookInventory();
        #region Đổi màu chữ console
            /// <summary>
            /// Đổi màu chữ sang xanh lá
            /// </summary>
            /// <param name="line"></param>
            static void Label (string line) {
                ForegroundColor = ConsoleColor.Green;
                Write(line);
                ForegroundColor = ConsoleColor.Cyan;
            }
    
            /// <summary>
            /// Đổi màu chữ sang đỏ
            /// </summary>
            /// <param name="line"></param>
            static void Warn (string line) {
                ForegroundColor = ConsoleColor.Red;
                Write(line);
                ForegroundColor = ConsoleColor.Cyan;
            }
        #endregion
        #region Khai báo các lớp
            //Lớp book
            public class Book {
                public int Id { get; set; }
                public required string ISBN {get;set;}
                public required string Title { get; set; }
                public string? Author { get; set; }
                public decimal Price { get; set; }
                public int Quantity { get; set; }
                public override string ToString() {
                    return $"{Id, -3} {Title, -30} {Author, -20} {ISBN, 20} {Price, 10} đ {Quantity, 20}"; 
                }
            }
    
            //Lớp BookInventory
            public class BookInventory {
                private readonly List<Book> books = new List<Book>();
                //Hàm lấy 1 book
                public Book? GetBook(int id) {
                    return books.Find(x => x.Id == id);
                }
                //Hàm thêm book
                public void AddBook (Book book) {
                    books.Add(book);
                }
                //Hàm xóa book 
                public void RemoveBook (Book book) {
                    books.RemoveAll(x => x.Id == book.Id);
                }
                //Hàm hiển thị toàn bộ book trong inventory
                public void DisplayBooks () {
                    Label($"{"Id", -3} {"Title", 20} {"Author", 20} {"ISBN", 20} {"Price", 22} {"Quantity", 20}\n");
                    foreach(var book in books) {
                        WriteLine(book.ToString());
                    }
                }
                //Tìm kiếm books, trả về list
                public List<Book> SearchBook (Func<Book, bool> predicate) {
                    if (predicate == null){
                        throw new ArgumentNullException(nameof(predicate), "Predicate function cannot be null."); 
                    }
                    return books.Where(predicate).ToList();
                }
                //cập nhật book
                public (Book?, int) UpdateBook(Book book, int quantity)
                {
                    // Validate input
                    if (book == null)
                    {
                        throw new ArgumentNullException(nameof(book), "Book cannot be null.");
                    }
    
                    if (quantity < 0)
                    {
                        throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
                    }
    
                    // Find the book in the collection (assuming 'books' is the list of books)
                    var existingBook = books.FirstOrDefault(b => b.Id == book.Id);
    
                    if (existingBook != null)
                    {
                        // Update the quantity
                        existingBook.Quantity += quantity;
                        return (existingBook, existingBook.Quantity);
                    }
    
                    // Return null if the book was not found
                    return (null, 0);
                }
                //sắp xếp books
                public List<Book> SortBooks (Comparison<Book> comparison) {
                    if (comparison == null) {
                        throw new ArgumentNullException(nameof(comparison), "Comparison criteria cannot be null.");
                    }
                    List<Book> sortedBook = new List<Book>(books);
                    sortedBook.Sort(comparison);
                    return sortedBook;
                }
            }
        #endregion
        #region Các hàm hỗ trợ nhập liệu
            /// <summary>
            /// Hàm hỗ trợ nhập string, param cuối nếu true sẽ check tồn tại của ISBN nhập vào
            /// </summary>
            /// <param name="label"></param>
            /// <param name="line"></param>
            /// <param name="checkISBN"></param>
            static void EnterString(string label, ref string line, bool checkISBN) {
                bool isValid = false;
                do {
                    Label(label);
                    string input = ReadLine();
                    if (string.IsNullOrEmpty(input)){
                        Warn("Please enter valid information!\n");
                    }
                    else {
                        if (checkISBN)
                            if (bookInventory.SearchBook(x => x.ISBN.Equals(input)).Count > 0) 
                                Warn("ISBN has existed. Please enter another ISBN.\n");
                            else {line = input; isValid = true;}
                        else {
                            line = input;
                            isValid = true;
                        }
                    }
                } while (!isValid);
            }
            //Hàm hỗ trợ nhập id
            static void EnterId (string label, ref int id) {
                bool isValid = false;
                do {
                    Label(label);
                    string input = ReadLine();
                    if (int.TryParse(input, out int _id)){
                        id = _id;
                        if (bookInventory.SearchBook(x => x.Id == _id).Count > 0) Warn("Id existed. Please enter another id!\n");
                        else if (id < 0) {
                            Warn("Book id cannot be negative. Please enter again!\n");
                        }
                        else isValid = true;
                    } else {
                        Warn("Please enter valid information!\n");
                    }
                } while (!isValid);
            }
            static void EnterIdToUpdate (string label, ref int id) {
                bool isValid = false;
                do {
                    Label(label);
                    string input = ReadLine();
                    if (int.TryParse(input, out int _id)){
                        id = _id;
                        if (bookInventory.SearchBook(x => x.Id == _id).Count == 0) Warn("Id not existed. Please enter again!\n");
                        else if (id < 0) {
                            Warn("Book id cannot be negative. Please enter again!\n");
                        }
                        else isValid = true;
                    } else {
                        Warn("Please enter valid information!\n");
                    }
                } while (!isValid);
            }
            /// <summary>
            /// Hàm hỗ trợ nhập int, param đầu là nhãn string, param thứ 2 là biến cần nhập giá trị
            /// </summary>
            /// <param name="label"></param>
            /// <param name="i"></param>
            static void EnterInt (string label, ref int i) {
                bool isValid = false;
                do {
                    Label(label);
                    string input = ReadLine();
                    if (int.TryParse(input, out int _i)){
                        i = _i;
                        if (!label.Contains("quantity to update") && i < 0) {
                            Warn("Input cannot be negative. Please enter again!\n");
                        }
                        else isValid = true;
                    } else {
                        Warn("Wrong input. Please enter again!\n");
                    }
                } while (!isValid);
            }
            //Hàm hỗ trợ nhập decimal
            static void EnterDecimal (string label, ref decimal d) {
                bool isValid = false;
                do {
                    Label(label);
                    string input = ReadLine();
                    if (int.TryParse(input, out int _d)){
                        d = _d;
                        if (d < 0) {
                            Warn("Input cannot be negative. Please enter again!\n");
                        }
                        else isValid = true;
                    } else {
                        Warn("Wrong input. Please enter again!\n");
                    }
                } while (!isValid);
            }
        #endregion
        #region Hàm xử lý mainmenu
            //Hiển thị mainmenu
            public static void MainMenu() {
                Label("========== Book Inventory ==========\n");
                Label("1. Add Book\n");
                Label("2. Remove Book\n");
                Label("3. Display Books\n");
                Label("4. Search Book\n");
                Label("5. Update Book\n");
                Label("6. Sort Books\n");
                Label("7. Exit\n");
                Label("Enter your choice: ");
            }
            //Xử lý mainmenu 
            public static void MainMenuManagement() {
                string choice = "";
                bool isValid = false;
                do {
                    MainMenu();
                    choice = ReadLine();
                    switch (choice) {
                        case "1": 
                            AddABookToInventory();
                            MainMenuManagement();
                            break;
                        case "2":
                            RemoveABookFromInventory();
                            MainMenuManagement();
                            break;
                        case "3": 
                            DisplayAllBooks();
                            MainMenuManagement();
                            break;
                        case "4":
                            SearchBookFromInventory();
                            MainMenuManagement();
                            break;
                        case "5":
                            UpdateABookFromInventory();
                            MainMenuManagement();
                            break;
                        case "6":
                            SortBooksInInventory();
                            MainMenuManagement();
                            break;
                        case "7":
                            Environment.Exit(0);
                            break;
                        default:
                            Warn("Invalid input. Please try again.\n");
                            break;
                    }
                } while (!isValid);
            }
            //Thêm sách
            static void AddABookToInventory() {
                int id = 0, quantity = 0;
                string isbn = "", title = "", author = "";
                decimal price = 0;
                //Nhập thông tin cho sách mới
                EnterId("Enter book id: ", ref id);
                EnterString("Enter book ISBN: ", ref isbn, true);
                EnterString("Enter book title: ", ref title, false);
                EnterString("Enter book author: ", ref author, false);
                EnterDecimal("Enter book price: ", ref price);
                EnterInt("Enter book quantity: ", ref quantity);
                //Thêm sách mới
                Book book = new Book {Id = id, ISBN = isbn, Title = title, Author = author, Price = price, Quantity = quantity};
                bookInventory.AddBook(book);
                Label("----- Added successfully -----\n");
            }
            //Hiển thị toàn bộ sách
            static void DisplayAllBooks() {
                bookInventory.DisplayBooks();
                Label("\n");
            }
            //Xóa sách
            static void RemoveABookFromInventory() {
                int id = 0;
                //Nhập id sách cần xóa
                bool isValid = false;
                do {
                    Label("Enter book id: ");
                    string input = ReadLine();
                    if (int.TryParse(input, out int _id)){
                        id = _id;
                        if (id < 0) {
                            Warn("Input cannot be negative. Please enter again!\n");
                        } else if (bookInventory.SearchBook(x => x.Id == id).Count == 0) {
                            Warn($"No book has id {id}. Please try again.\n");
                        }
                        else isValid = true;
                    } else {
                        Warn("Wrong input. Please enter again!\n");
                    }
                } while (!isValid);
                //Tiến hành xóa
                bookInventory.RemoveBook(new Book{Id=id, ISBN="", Title=""});
                Label("----- Removed successfully -----\n");
            }
            //Tìm kiếm sách
            static void SearchBookFromInventory() {
                //Nhập từ khóa để tìm
                string keyword = "";
                EnterString("Enter search keyword: ", ref keyword, false);
                //Hiển thị kết quả tìm kiếm
                Label("Search result: \n");
                var searchedBook = bookInventory.SearchBook(x => x.Title.Contains(keyword) || x.Author.Contains(keyword) || x.ISBN.Contains(keyword));
                if (searchedBook.Count == 0) Warn("No result.\n");
                else {
                    Label($"{"Id", -3} {"Title", 20} {"Author", 20} {"ISBN", 20} {"Price", 22} {"Quantity", 20}\n");
                    foreach(var book in searchedBook) {
                        WriteLine(book.ToString());
                    }
                }
            }
            //Cập nhật sách, theo như mẫu là thêm quantity sách vào giá, không phải cập nhật bản ghi
            static void UpdateABookFromInventory() {
                int quantityToAdd = 0;
                int id = 0;
                EnterIdToUpdate("Enter book id: ", ref id);
                EnterInt("Enter quantity to update: ", ref quantityToAdd);
                var temp = bookInventory.UpdateBook(new Book{Id = id, ISBN = "", Title = ""}, quantityToAdd);
                Label($"Book updated successfully. \nNew quantity: {temp.Item2}\n");
            }
            //Sắp xếp sách
            static void SortBooksInInventory() {
                var sortedBookInventory = bookInventory.SortBooks((x, y) => string.Compare(x.Title, y.Title));
                Label($"{"Id", -3} {"Title", 20} {"Author", 20} {"ISBN", 20} {"Price", 22} {"Quantity", 20}\n");
                    foreach(var book in sortedBookInventory) {
                        WriteLine(book.ToString());
                    }
            }
        #endregion
        //Dữ liệu thêm vào để test
        static void BookInventory_UnitTest() {
            //AddBook
            bookInventory.AddBook(new Book{Id = 1, ISBN = "978-0-306-40615-7", Author = "Thomas H, Cormen", Title = "Introduction to Algorithm", Price = 100, Quantity = 10});
            bookInventory.AddBook(new Book{Id = 2, ISBN = "978-0-13-595705-9", Author = "Robert C, Martin", Title = "Clean code", Price = 50, Quantity = 5});
            bookInventory.AddBook(new Book{Id = 3, ISBN = "978-0-321-87758-1", Author = "Andrew Hunt", Title = "The pragmatic Programmer", Price = 75, Quantity = 7});
            bookInventory.AddBook(new Book{Id = 4, ISBN = "978-0-23-28347-9", Author = "Fujiko F, Fujio", Title = "Doraemon", Price = 99, Quantity = 22});
            bookInventory.AddBook(new Book{Id = 5, ISBN = "978-0-23-28347-99", Author = "Fujiko F, Fujio", Title = "Doraemon", Price = 99, Quantity = 22});
            //RemoveBook
            bookInventory.RemoveBook(new Book{Id = 5, ISBN = "", Title = ""});
        }
        public static void Main(string[] args)
        {
            BookInventory_UnitTest();
            MainMenuManagement();
            ReadLine();
        }
    }
}


//(c) 202523021_HoangXuanQuy
