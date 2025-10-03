namespace ExTrack.Checks.Models;

public record CheckInfo(
    string             ShopName,
    string             ShopAddress,
    DateTime           CheckDate,
    long               TotalSum,
    List<CheckProduct> Products);

public record CheckProduct(string Name, long Price, long TotalPrice, double Quantity);
