namespace votacionCneEduardo.Models;

public class VoteStatistics
{
    public int TotalVotantes { get; set; }
    public int TotalVotosEmitidos { get; set; }
    public double PorcentajeParticipacion { get; set; }
    public Dictionary<string, int> VotosPorCandidato { get; set; } = new();
    public List<int> TendenciaTemporal { get; set; } = new();
}
