using NetTopologySuite.Geometries;
using System.Collections.Generic;

public static class PolylineDecoder
{
    public static List<Coordinate> DecodePolyline(string encoded)
    {
        var polylineChars = encoded.ToCharArray();
        int index = 0;
        int currentLat = 0;
        int currentLng = 0;
        var coordinates = new List<Coordinate>();

        while (index < polylineChars.Length)
        {
            currentLat += DecodeNextValue(polylineChars, ref index);
            currentLng += DecodeNextValue(polylineChars, ref index);
            coordinates.Add(new Coordinate(currentLng / 1e5, currentLat / 1e5)); // Note: X = longitude, Y = latitude
        }

        return coordinates;
    }

    private static int DecodeNextValue(char[] polylineChars, ref int index)
    {
        int result = 0;
        int shift = 0;
        int b;

        do
        {
            b = polylineChars[index++] - 63;
            result |= (b & 0x1f) << shift;
            shift += 5;
        } while (b >= 0x20);

        return ((result & 1) != 0) ? ~(result >> 1) : (result >> 1);
    }
    public static string ToLineStringWkt(List<Coordinate> coordinates)
    {
        if (coordinates == null || coordinates.Count == 0)
        {
            throw new ArgumentException("Coordinates list is empty");
        }

        // Build WKT string in "LINESTRING(lng lat, lng lat, ...)" format
        var wkt = "LINESTRING(";
        for (int i = 0; i < coordinates.Count; i++)
        {
            var coord = coordinates[i];
            wkt += $"{coord.X} {coord.Y}";
            if (i < coordinates.Count - 1)
            {
                wkt += ",";
            }
        }
        wkt += ")";
        return wkt;
    }
}
