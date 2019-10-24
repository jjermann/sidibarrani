namespace SidiBarraniCommon.Info
{
    public static class GameStageInfoExtensionMethods
    {
        public static GameStageInfo GetRelativeGameStageInfo(this GameStageInfo gameStageInfo, PlayerGroupInfo playerGroupInfo, string playerId)
        {
            var isCurrentTeamTeam1 = playerGroupInfo.GetTeamOfPlayer(playerId).TeamId == playerGroupInfo.Team1.TeamId;
            var relativePlayerGroupInfo = playerGroupInfo.GetRelativePlayerGroupInfo(playerId);
            var relativeGameStageInfo = (GameStageInfo)(gameStageInfo?.Clone());
            if (relativeGameStageInfo.CurrentPlayer != null)
            {
                relativeGameStageInfo.CurrentPlayer = playerGroupInfo.GetRelativePlayerInfo(playerId, gameStageInfo.CurrentPlayer.PlayerId);
            }
            if (relativeGameStageInfo.CurrentBetActionList != null)
            {
                foreach (var betAction in relativeGameStageInfo.CurrentBetActionList)
                {
                    betAction.PlayerInfo = playerGroupInfo.GetRelativePlayerInfo(playerId, betAction.PlayerInfo.PlayerId);
                }
            }
            if (relativeGameStageInfo?.CurrentBetResult?.BettingTeam != null)
            {
                relativeGameStageInfo.CurrentBetResult.BettingTeam = playerGroupInfo.GetRelativeTeamInfo(playerId, gameStageInfo.CurrentBetResult.BettingTeam.TeamId);
            }
            if (relativeGameStageInfo?.StickRoundInfoList != null)
            {
                foreach (var stickRoundInfo in relativeGameStageInfo.StickRoundInfoList)
                {
                    if (stickRoundInfo?.PlayActionList != null)
                    {
                        foreach (var playAction in stickRoundInfo.PlayActionList)
                        {
                            playAction.PlayerInfo = playerGroupInfo.GetRelativePlayerInfo(playerId, playAction.PlayerInfo.PlayerId);
                        }
                    }
                    if (stickRoundInfo?.StickResult?.Winner != null)
                    {
                        stickRoundInfo.StickResult.Winner = playerGroupInfo.GetRelativePlayerInfo(playerId, stickRoundInfo.StickResult.Winner.PlayerId);
                    }
                }
            }
            if (relativeGameStageInfo?.CurrentPlayResult != null)
            {
                relativeGameStageInfo.CurrentPlayResult.PlayerGroupInfo = relativePlayerGroupInfo;
                if (!isCurrentTeamTeam1)
                {
                    relativeGameStageInfo.CurrentPlayResult.Team1Score = gameStageInfo.CurrentPlayResult.Team2Score;
                    relativeGameStageInfo.CurrentPlayResult.Team2Score = gameStageInfo.CurrentPlayResult.Team1Score;
                }
            }
            if (relativeGameStageInfo?.RoundResultList != null)
            {
                foreach (var roundResult in relativeGameStageInfo?.RoundResultList)
                {
                    if (roundResult != null)
                    {
                        roundResult.PlayerGroupInfo = relativePlayerGroupInfo;
                        roundResult.WinningTeam = playerGroupInfo.GetRelativeTeamInfo(playerId, roundResult.WinningTeam.TeamId);
                        if (!isCurrentTeamTeam1)
                        {
                            var team1Score = roundResult.Team2FinalScore;
                            var team2Score = roundResult.Team1FinalScore;
                            roundResult.Team1FinalScore = team1Score;
                            roundResult.Team2FinalScore = team2Score;
                        }
                    }
                }
            }
            if (relativeGameStageInfo?.GameResult != null)
            {
                relativeGameStageInfo.GameResult.PlayerGroupInfo = relativePlayerGroupInfo;
                relativeGameStageInfo.GameResult.WinningTeam = playerGroupInfo.GetRelativeTeamInfo(playerId, relativeGameStageInfo.GameResult.WinningTeam.TeamId);
                if (!isCurrentTeamTeam1)
                {
                    var team1Score = relativeGameStageInfo.GameResult.Team2FinalScore;
                    var team2Score = relativeGameStageInfo.GameResult.Team1FinalScore;
                    relativeGameStageInfo.GameResult.Team1FinalScore = team1Score;
                    relativeGameStageInfo.GameResult.Team2FinalScore = team2Score;
                }
            }

            return relativeGameStageInfo;
        }
    }
}