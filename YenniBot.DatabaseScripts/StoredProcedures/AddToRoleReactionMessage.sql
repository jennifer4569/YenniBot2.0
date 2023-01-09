USE YenniBotDB_V2;
DROP PROCEDURE IF EXISTS `AddToRoleReactionMessage`;

DELIMITER $$
CREATE PROCEDURE `AddToRoleReactionMessage`(
	pMessageId BIGINT,
    pRoleId BIGINT,
    pEmote VARCHAR(255),
    pIsEmoji TINYINT
)
BEGIN
	INSERT INTO RoleReactionMessage 
		(MessageId, RoleId, Emote, IsEmoji) 
    VALUES 
		(pMessageId, pRoleId, pEmote, pIsEmoji);
END
$$
