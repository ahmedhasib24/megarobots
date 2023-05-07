using UnityEngine;
using System.Collections;

namespace PhotonEnums
{
    public class GameType
    {
        //Custom room properties
        public static string BlackJack = "blackjack";
        public static string BuuGyee = "buu_gyee";
        public static string DiceTraditional = "dice_traditional";
        public static string DiceInternational = "dice_international";
        public static string KoeMee = "koe_mee";
        public static string ShanKoeMee = "shan_koe_mee";
        public static string Show = "show";
        public static string Roulette = "roulette";
        public static string Slot = "slot";
        public static string TexasPoker = "texas_poker";

    }

    public class GameSettings
    {
        //Custom room properties
        public static string Easy = "easy";
        public static string Medium = "medium";
        public static string Hard = "hard";
        public static string Advanced = "advanced";
        public static string Pro = "pro";
        public static string Extreme = "extreme";
        public static string Insane = "insane";
    }

    public class Room
    {
        //Custom room properties
        public static string RobotCount = "robot_count";
        public static string MasterClientID = "master_client_id";
        public static string AttackRobotOrder = "attack_robot_order";
        //public static string VersionCode = "version_code";
        //public static string GameType = "game_type";
        //public static string Environment = "environment";
        //public static string Difficulty = "difficulty";
        //public static string GameStarted = "game_started";
        //public static string RoundCompleted = "round_completed";
        //public static string Slots = "slots";
        //public static string WinnerCount = "winner_count";
        //public static string MinimumBet = "minimum_bet";
        //public static string BankerDebts = "banker_debts";
        //public static string MaxBankMoney = "max_bank_money";
        //public static string BankMoney = "bank_money";
        //public static string RoundMoney = "round_money";
        //public static string TempBankMoney = "temp_bank_money";
        //public static string CurBetAmount = "cur_bet_amount";
        //public static string MoneyRaised = "money_raised";
        //public static string MoneyRaisedPlayer = "money_raised_player";
        //public static string FirstCallPlayer = "first_call_player";
        //public static string LastDecidingPlayer = "last_deciding_player";
        //public static string Pausetable = "pause_table";
        //public static string RoundDebts = "round_debts";
        //public static string AIPlayer = "ai_player";
        //public static string Debts = "debts";
        //public static string CurCardIndex = "cur_card_index";
        //public static string BankerCardIndex = "banker_card_index";
        //public static string FirstTimeCards = "first_time_cards";
        //public static string RaceComplete = "cards";
        //public static string BankerCards = "banker_cards";
        //public static string Password = "password";
        //public static string DisconnectedPlayers = "disconnected_players";
        //public static string PlayerIDs = "player_ids";
        //public static string CardTypes = "card_types";
        //public static string CardPoints = "card_points";
        //public static string BetMoneys = "bet_moneys";
        //public static string CanJoinRoom = "can_join_room";
        //public static string CanPlaceBet = "can_place_bet";
        //public static string PlacingBet = "placing_bet";
        //public static string IsRoomClosed = "is_room_closed";
        //public static string MyTestingString = "my_testing_string";
        //public static string AvaterAssignRequests = "avater_assign_requests";
        //public static string BetType = "bet_type";
        //public static string FBID = "fb_id";

        //Card Distributings
        public static string IsDistributingCards = "is_distributing_cards";
        public static string CurDistributingRound = "cur_distributing_round";

        //Show
        public static string Indexes = "indexes";
        public static string CurRoundIndex = "cur_round_index";
        public static string CurPlayerIndex = "cur_player_index";
        public static string CurSlotIndex = "cur_slot_index";
        public static string PlayerWithMaxAmount = "player_with_max_amount";
        public static string PlayerWithMinAmount = "player_with_min_amount";
        public static string MaxAmountVal = "max_amount_val";
        public static string MinAmountVal = "min_amount_val";

        //Black Jack
        public static string WarningStarted = "warning_started";
        public static string WarningCount = "warning_count";
        public static string NumBets = "num_bets";
        public static string CurDealer = "cur_dealer";
        public static string CurPlayer = "cur_player";
    }

    public class Player
    {
        //Player custom properties
        public static string Name = "name";
        public static string Ready = "ready";
        public static string JoinedGame = "joined_game";
        public static string Money = "money";
        public static string Scores = "scores";
        public static string PictureURL = "picture_url";
        public static string AvaterID = "avater_id";
        public static string FacebookID = "fb_id";
        public static string UserID = "user_id";
        public static string Role = "role";
        public static string PlayerID = "player_id";
        public static string RaceComplete = "race_complete";
        public static string AttackSelected = "attack_selected";
        public static string RobotCount = "robot_count";

        public static string Robot1 = "robot_1";
        public static string Robot2 = "robot_2";
        public static string Robot3 = "robot_3";

        public static string AttackTargetString1 = "attack_target_string1";
        public static string AttackTargetString2 = "attack_target_string2";
        public static string AttackTargetString3 = "attack_target_string3";

        public static string Head1 = "head_1";
        public static string Head2 = "head_2";
        public static string Head3 = "head_3";

        public static string LeftArm1 = "left_arm_1";
        public static string LeftArm2 = "left_arm_2";
        public static string LeftArm3 = "left_arm_3";

        public static string RightArm1 = "right_arm_1";
        public static string RightArm2 = "right_arm_2";
        public static string RightArm3 = "right_arm_3";

        public static string Leg1 = "leg_1";
        public static string Leg2 = "leg_2";
        public static string Leg3 = "leg_3";
    }

    public class RPC
    {

        public static string PlayerCheckedInRPC = "PlayerCheckedInRPC";
        public static string SkipPlayerRPC = "SkipPlayerRPC";
        public static string SyncBankMoneyRPC = "SyncBankMoneyRPC";
        public static string KickPlayerRPC = "KickPlayerRPC";
        public static string CreditsUpdateRPC = "CreditsUpdateRPC";
        public static string AddBankMoneyRPC = "AddBankMoneyRPC";
        public static string CloseRoomRPC = "CloseRoomRPC";
        public static string SendMessageRPC = "SendMessageRPC";

        //Social
        public static string RequestToJoinRPC = "RequestToJoinRPC";
        public static string RespondRequestToJoinRPC = "RespondRequestToJoinRPC";

        public static string RequestPlayerInfoRPC = "RequestPlayerInfoRPC";
        public static string RequestAssignAvaterRPC = "RequestAssignAvaterRPC";
        public static string UpdatePlayerInfoRPC = "UpdatePlayerInfoRPC";
        public static string AddChipsRPC = "AddChipsRPC";
        public static string RemoveChipsRPC = "RemoveChipsRPC";

        public static string AssignAvaterRPC = "AssignAvaterRPC";
        public static string AvaterAssignedRPC = "AvaterAssignedRPC";
        public static string UpdateBankMoneyRPC = "UpdateBankMoneyRPC";
        public static string ResetMoneyPaidRPC = "ResetMoneyPaidRPC";
        public static string PrepareRoundRPC = "PrepareRoundRPC";
        public static string ShowConnectedBoxRPC = "ShowConnectedBoxRPC";
        public static string ShowDisconnectedBoxRPC = "ShowDisconnectedBoxRPC";
        public static string ShowChangedMasterClientRPC = "ShowChangedMasterClientRPC";
        public static string CalculateWinningMoneyRPC = "CalculateWinningMoneyRPC";
        public static string ShowWonLostRPC = "ShowWonLostRPC";
        public static string DivideWinningMoneyRPC = "DivideWinningMoneyRPC";
        public static string RemoveAvaterFromSlotRPC = "RemoveAvaterFromSlotRPC";
        public static string ShowEmojiRPC = "ShowEmojiRPC";
        public static string RecalculateDebtsRPC = "RecalculateDebtsRPC";

        public static string GameStartedRPC = "GameStartedRPC";
        public static string GameStartedNewRPC = "GameStartedNewRPC";
        public static string RestartGameRoundRPC = "RestartGameRoundRPC";
        public static string GoToNextPlayerTurnRPC = "GoToNextPlayerTurnRPC";
        public static string GameRoundFinishedRPC = "GameRoundFinishedRPC";
        public static string GameRoundFinishedEarlyRPC = "GameRoundFinishedEarlyRPC";
        public static string CardScoresReceivedRPC = "CardScoresReceivedRPC";
        public static string ResetPlaceholderCardsRPC = "ResetPlaceholderCardsRPC";

        //Dice Game
        public static string RollDiceRPC = "RollDiceRPC";
        public static string RollDiceEndedRPC = "RollDiceEndedRPC";

        //Bu Gyee Game
        public static string CardsShuffledRPC = "CardsShuffledRPC";
        public static string CardReceivedRPC = "CardReceivedRPC";
        public static string CallEarlyRPC = "CallEarlyRPC";
        public static string CardDrawRPC = "CardDrawRPC";
        public static string BankerCardReceivedRPC = "BankerCardReceivedRPC";
        public static string ShowCardPointsRPC = "ShowCardPointsRPC";
        public static string HideCardPointsRPC = "HideCardPointsRPC";
        public static string ShowAllCardPointsRPC = "ShowAllCardPointsRPC";
        public static string ShowPlayersCardPointsRPC = "ShowPlayersCardPointsRPC";
        public static string ShowPlayerCardPointsRPC = "ShowPlayerCardPointsRPC";
        public static string ShowBankerCardPointsRPC = "ShowBankerCardPointsRPC";
        public static string StartNextRoundRPC = "StartNextRoundRPC";

        //Show Game
        public static string ShowRaiseFoldRPC = "ShowRaiseFoldRPC";
        public static string GoToNextDecisionRPC = "GoToNextDecisionRPC";
        public static string StartBetTimerRPC = "StartBetTimerRPC";
        public static string StartTimerRPC = "StartTimerRPC";
        public static string StopTimerRPC = "StopTimerRPC";
        public static string PlaceBetRPC = "PlaceBetRPC";
        public static string CheckFirstCardsRPC = "CheckFirstCardsRPC";

        //Blackjack and Shan
        public static string ResetPlayerPropertiesRPC = "ResetPlayerPropertiesRPC";
        public static string ShowCardBetRPC = "ShowCardBetRPC";
        //public static string ShowPlaceBetRPC = "ShowPlaceBetRPC";
        public static string ShowCardDecisionRPC = "ShowCardDecisionRPC";
        public static string CalculateScoresRPC = "CalculateScoresRPC";
        public static string WarningStartedRPC = "WarningStartedRPC";
        public static string ChangeDealerRPC = "ChangeDealerRPC";
        public static string RefreshBankMoneyRPC = "RefreshBankMoneyRPC";
        public static string RefreshCreditsRPC = "RefreshCreditsRPC";
        public static string UpdateScoresRPC = "UpdateScoresRPC";
        public static string RefreshOfflineMoneyRPC = "RefreshOfflineMoneyRPC";
        public static string SyncOfflineMoneyRPC = "SyncOfflineMoneyRPC";
        public static string BetPlacedRPC = "BetPlacedRPC";
        public static string DrawCardRPC = "DrawCardRPC";
        public static string CheckRPC = "CheckRPC";
        public static string SurrenderRPC = "SurrenderRPC";
        public static string EarlyWinnersRPC = "EarlyWinnersRPC";

        //Sync RaceComplete for the first time
        public static string SyncCardsFirstTimeRPC = "SyncCardsFirstTimeRPC";
    }
}