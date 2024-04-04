using System.ComponentModel;

namespace Seats;

public abstract class Space
{
    public int Row { get; set; }

    public int Column { get; set; }
}

// TODO: Implement INotifyPropertyChanged appropriately for the following class
public class Seat : Space
{
    public bool Occupied { get; set; }

    public decimal Price => Row switch
    {
        >= 8 => 100,
        >= 5 => 75,
        <= 1 => 25,
        _ => 50
    };
}

public class WheelchairSpace : Seat{ }

public class Aisle : Space { }

public class MainWindowViewModel : INotifyPropertyChanged
{
    // TODO: Think about whether the following property must be an ObservableCollection
    public List<List<Space>> Spaces { get; } = [];

    public DelegateCommand SeatClick { get; }

    // TODO: Think about whether the following property must be an ObservableCollection
    public List<Seat> OrderedSeats { get; } =
        [
            // Note that the following data is only for demonstration purposes.
            // Remove it when you implement the real logic.
            // The user can add or remove seats from the order through the UI.
            new() { Row = 1, Column = 1 },
        ];

    public MainWindowViewModel()
    {
        SeatClick = new DelegateCommand(this, OnSeatClick, CanClickSeat);

        // Fill with data
        Spaces = BuildSpacesFromFloorplan();
        OccupyRandomSeats(Spaces, 20);
    }

    private bool CanClickSeat(object? obj)
    {
        var seat = obj as Seat;
        if (seat is null) { return false; }

        // TODO: Add the necessary logic to determine whether the seat can be clicked.
        // The seat (regular or wheelchair) can be clicked if it is not already occupied.
        return true; // Remove this line when you implement the real logic.
    }

    private void OnSeatClick(object? obj)
    {
        if (obj is not Seat seat) { return; }

        // TODO: Add the necessary logic to handle the seat click.
        // If clicked, the seat should be added to the OrderedSeats collection.
        // Additionally, the seat must be marked as occupied. This must turn
        // the seat in the UI from green to red.
        throw new NotImplementedException();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    // The following method fills the floorplan with seats, aisles, and wheelchair spaces.
    // DO NOT MODIFY THIS METHOD. It is ok as it is.
    public static List<List<Space>> BuildSpacesFromFloorplan()
    {
        const int NUMBER_OF_ROWS = 10;
        const int NUMBER_OF_COLUMNS = 15;
        const int AISLE_WIDTH = 2;
        const int SEATS_FROM_AISLE_TO_WALL = 2;
        const int ROW_WITH_WHEELCHAIR_SPACE = 5;
        const int AISLE_ROW = 4;

        var spaces = new List<List<Space>>();
        for (var row = 0; row < NUMBER_OF_ROWS; row++)
        {
            var rowSpaces = new List<Space>();
            for (var col = 0; col < NUMBER_OF_COLUMNS; col++)
            {
                if (row + 1 == AISLE_ROW)
                {
                    rowSpaces.Add(new Aisle { Row = row, Column = col });
                }
                else if (col + 1 is > SEATS_FROM_AISLE_TO_WALL and <= SEATS_FROM_AISLE_TO_WALL + AISLE_WIDTH
                    || col + 1 is > NUMBER_OF_COLUMNS - SEATS_FROM_AISLE_TO_WALL - AISLE_WIDTH and <= NUMBER_OF_COLUMNS - SEATS_FROM_AISLE_TO_WALL)
                {
                    rowSpaces.Add(new Aisle { Row = row, Column = col });
                }
                else
                {
                    if (col + 1 is <= SEATS_FROM_AISLE_TO_WALL or > NUMBER_OF_COLUMNS - SEATS_FROM_AISLE_TO_WALL && row + 1 == ROW_WITH_WHEELCHAIR_SPACE)
                    {
                        rowSpaces.Add(new WheelchairSpace { Row = row, Column = col });
                    }
                    else
                    {
                        rowSpaces.Add(new Seat { Row = row, Column = col });
                    }
                }
            }

            spaces.Add(rowSpaces);
        }

        return spaces;
    }

    // The following method randomly occupies a given number of seats.
    // DO NOT MODIFY THIS METHOD. It is ok as it is.
    public static void OccupyRandomSeats(List<List<Space>> spaces, int numberOfSeatsToOccupy)
    {
        var random = new Random(11);
        var seats = spaces.SelectMany(row => row.OfType<Seat>()).ToList();
        for (var i = 0; i < numberOfSeatsToOccupy; i++)
        {
            var seat = seats[random.Next(seats.Count)];
            seat.Occupied = true;
            seats.Remove(seat);
        }
    }
}
