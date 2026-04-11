using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarthQuake.Persistence.Models;

public class CrustStress
{
    public string? ID { get; set; }
    public string? ISC_ID { get; set; }
    public string? SITE { get; set; }
    public decimal? LAT { get; set; }
    public decimal? LON { get; set; }
    public decimal? AZI { get; set; }
    public string? TYPE { get; set; }
    public float? DEPTH { get; set; }
    public string? QUALITY { get; set; }
    public string? REGIME { get; set; }
    public string? LOCALITY { get; set; }
    public string? COUNTRY { get; set; }
    public string? DATE { get; set; }
    public string? TIME { get; set; }
    public int? NUMBER { get; set; }
    public decimal? SD { get; set; }
    public decimal? TOT_LEN { get; set; }
    public string? VENT { get; set; }
    public decimal? TOP { get; set; }
    public decimal? BOT { get; set; }
    public string? ANISOTROPY { get; set; }
    public string? METHOD { get; set; }
    public decimal? S1AZ { get; set; }
    public decimal? S1PL { get; set; }
    public decimal? S2AZ { get; set; }
    public decimal? S2PL { get; set; }
    public decimal? S3AZ { get; set; }
    public string? S3PL { get; set; }
    public string? MAG_TYPE { get; set; }
    public string? EQ_MAG { get; set;}
    public string? CRUST { get; set; }
    public string? REF1 { get; set; }
    public string? REF2 { get; set; }
    public string? REF3 { get; set; }
    public string? REF4 { get; set; }
    public string? REF5 { get; set; }
    public string? REF6 { get; set; }
    public string? COMMENT { get; set; }
    public string? PLATE { get; set; }
    public decimal? DIST {  get; set; }
}
