using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarthQuake.Persistence.Models;

public class CrustStress
{
    public string Id { get; set; }
    public string Isc_Id { get; set; }
    public string Site { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Azimuth { get; set; }
    public string Type { get; set; }
    public float Depth { get; set; }
    public char Quality { get; set; }
    public string Method { get; set; }
    public string Regime { get; set; }
    public string Locality { get; set; }
    public string Country { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int Number { get; set; }
    public decimal SD { get; set; }
    public decimal TotalLength { get; set; }
    public string Vent { get; set; }
    public decimal TopDepth { get; set; }
    public decimal BottomDepth { get; set; }
    public string Anisotropy { get; set; }
    public decimal S1az { get; set; }
    public decimal S1pl { get; set; }
    public decimal S2az { get; set; }
    public decimal S2pl { get; set; }
    public decimal S3az { get; set; }
    public decimal S3pl { get; set; }
    public string MagnitudeType { get; set; }
    public decimal EarthquakeMagnitude { get; set;}
    public string Crust { get; set; }
    public string Reference1 { get; set; }
    public string Reference2 { get; set; }
    public string Reference3 { get; set; }
    public string Reference4 { get; set; }
    public string Reference5 { get; set; }
    public string Reference6 { get; set; }
    public string Comment { get; set; }
    public string Plate { get; set; }
    public decimal Dist {  get; set; }
}
