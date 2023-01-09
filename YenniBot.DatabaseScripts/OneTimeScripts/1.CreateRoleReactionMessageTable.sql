USE YenniBotDB_V2;
CREATE TABLE RoleReactionMessage (
	Id INT AUTO_INCREMENT PRIMARY KEY,
    MessageId BIGINT,
    RoleId BIGINT,
    Emote VARCHAR(255),
    IsEmoji TINYINT
);