using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models;

public class Patron
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public bool IsActive { get; set; }
    public ICollection<Checkout> Checkouts { get; set; }

    public decimal Balance
    {
        get
        {
            decimal balance = 0M;
            decimal lateFeePerDay = 0.50M;

            foreach (var checkout in Checkouts)
            {
                // Only calculate balance for unpaid checkouts
                if (!checkout.Paid)
                {
                    // Calculate the due date
                    DateTime dueDate = checkout.CheckoutDate.AddDays(checkout.Material.MaterialType.CheckoutDays);

                    // Use return date if available, otherwise use today's date
                    DateTime returnDate = checkout.ReturnDate ?? DateTime.Today;

                    // Calculate days late
                    int daysLate = (returnDate - dueDate).Days;

                    // If late, add the fee to balance
                    if (daysLate > 0)
                    {
                        decimal fee = daysLate * lateFeePerDay;
                        balance += fee;
                    }
                }
            }

            return balance;
        }
    }
}