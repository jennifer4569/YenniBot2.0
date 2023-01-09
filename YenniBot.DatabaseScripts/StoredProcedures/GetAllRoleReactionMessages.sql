USE YenniBotDB_V2;
DROP PROCEDURE IF EXISTS `GetAllRoleReactionMessages`;

DELIMITER $$
CREATE PROCEDURE `GetAllRoleReactionMessages`()
BEGIN
	SELECT 
		MessageId, 
        RoleId, 
        Emote, 
        IsEmoji
    FROM
		RoleReactionMessage;
END
$$