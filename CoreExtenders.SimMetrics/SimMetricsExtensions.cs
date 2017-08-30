using SimMetrics.Metric;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreExtenders
{
    public static class SimMetricsExtensions
    {
        private static double ApproximatelyEquals(this string firstWord, string secondWord, SimMetricType simMetricType = SimMetricType.Levenstein)
        {
            switch (simMetricType)
            {
                case SimMetricType.BlockDistance:
                    var sim2 = new BlockDistance();
                    return sim2.GetSimilarity(firstWord, secondWord);
                case SimMetricType.ChapmanLengthDeviation:
                    var sim3 = new ChapmanLengthDeviation();
                    return sim3.GetSimilarity(firstWord, secondWord);
                case SimMetricType.CosineSimilarity:
                    var sim4 = new CosineSimilarity();
                    return sim4.GetSimilarity(firstWord, secondWord);
                case SimMetricType.DiceSimilarity:
                    var sim5 = new DiceSimilarity();
                    return sim5.GetSimilarity(firstWord, secondWord);
                case SimMetricType.EuclideanDistance:
                    var sim6 = new EuclideanDistance();
                    return sim6.GetSimilarity(firstWord, secondWord);
                case SimMetricType.JaccardSimilarity:
                    var sim7 = new JaccardSimilarity();
                    return sim7.GetSimilarity(firstWord, secondWord);
                case SimMetricType.Jaro:
                    var sim8 = new Jaro();
                    return sim8.GetSimilarity(firstWord, secondWord);
                case SimMetricType.JaroWinkler:
                    var sim9 = new JaroWinkler();
                    return sim9.GetSimilarity(firstWord, secondWord);
                case SimMetricType.MatchingCoefficient:
                    var sim10 = new MatchingCoefficient();
                    return sim10.GetSimilarity(firstWord, secondWord);
                case SimMetricType.MongeElkan:
                    var sim11 = new MongeElkan();
                    return sim11.GetSimilarity(firstWord, secondWord);
                case SimMetricType.NeedlemanWunch:
                    var sim12 = new NeedlemanWunch();
                    return sim12.GetSimilarity(firstWord, secondWord);
                case SimMetricType.OverlapCoefficient:
                    var sim13 = new OverlapCoefficient();
                    return sim13.GetSimilarity(firstWord, secondWord);
                case SimMetricType.QGramsDistance:
                    var sim14 = new QGramsDistance();
                    return sim14.GetSimilarity(firstWord, secondWord);
                case SimMetricType.SmithWaterman:
                    var sim15 = new SmithWaterman();
                    return sim15.GetSimilarity(firstWord, secondWord);
                case SimMetricType.SmithWatermanGotoh:
                    var sim16 = new SmithWatermanGotoh();
                    return sim16.GetSimilarity(firstWord, secondWord);
                case SimMetricType.SmithWatermanGotohWindowedAffine:
                    var sim17 = new SmithWatermanGotohWindowedAffine();
                    return sim17.GetSimilarity(firstWord, secondWord);
                case SimMetricType.ChapmanMeanLength:
                    var sim18 = new ChapmanMeanLength();
                    return sim18.GetSimilarity(firstWord, secondWord);
                default:
                    var sim1 = new Levenstein();
                    return sim1.GetSimilarity(firstWord, secondWord);
            }
        }

        public static string LessSimilarity(this string source, string[] targets, bool ignoreCase = false, double minThreshold = 0,
                    double maxThreshold = 1, SimMetricType simMetricType = SimMetricType.Levenstein)
        {
            var count = source.Similarity(targets, ignoreCase, minThreshold, maxThreshold, simMetricType).Count;
            return count > 0 ? source.Similarity(targets, ignoreCase, minThreshold, maxThreshold, simMetricType).FirstOrDefault().Item2 : null;
        }

        public static string MostSimilarity(this string source, string[] targets, bool ignoreCase = false, double minThreshold = 0,
                    double maxThreshold = 1, SimMetricType simMetricType = SimMetricType.Levenstein)
        {
            var count = source.Similarity(targets, ignoreCase, minThreshold, maxThreshold, simMetricType).Count;
            return count > 0 ? source.Similarity(targets, ignoreCase, minThreshold, maxThreshold, simMetricType).LastOrDefault().Item2 : null;
        }

        public static double Similarity(this string source, string target, bool ignoreCase = false, SimMetricType simMetricType = SimMetricType.Levenstein)
        {
            source = ignoreCase ? source.ToLowerInvariant() : source;
            target = ignoreCase ? target.ToLowerInvariant() : target;
            return source.ApproximatelyEquals(target, simMetricType);
        }

        public static List<Tuple<double, string>> Similarity(this string source, string[] targets, bool ignoreCase = false,
                    double minThreshold = 0, double maxThreshold = 1,
                    SimMetricType simMetricType = SimMetricType.Levenstein, bool orderByAscending = true)
        {
            var all = new List<Tuple<double, string>>();
            foreach (var target in targets)
            {
                var num = source.Similarity(target, ignoreCase, simMetricType);
                if (num >= minThreshold && maxThreshold <= num)
                    all.Add(new Tuple<double, string>(num, target));
            }
            all.Sort();
            if (!orderByAscending)
                all.Reverse();

            return all;
        }
    }
}
