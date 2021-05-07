//Timeline
CALL {
    MATCH (:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46'})-[:FOLLOWS]->(user:User)<-[:KWEETED_BY]-(kweet:Kweet)
    MATCH (userProfile:UserProfile)<-[:DESCRIBED_BY]-(user)
    RETURN userProfile, user, kweet
    UNION
    MATCH (userProfile:UserProfile)<-[:DESCRIBED_BY]-(user:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46'})<-[:KWEETED_BY]-(kweet:Kweet)
    RETURN userProfile, user, kweet
}
WITH userProfile, user, kweet
ORDER BY kweet.createdDateTime DESC
SKIP 0
LIMIT 25
CALL {
    WITH kweet
    OPTIONAL MATCH (liker:User)<-[:LIKED_BY]-(kweet)
    RETURN liker
}
CALL {
    WITH kweet
    OPTIONAL MATCH (i:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46'})<-[:LIKED_BY]-(kweet)
    RETURN (i.id IS NOT NULL) as liked
}
RETURN user.id, user.userName, user.userDisplayName, userProfile.pictureUrl, kweet.createdDateTime, kweet.id, kweet.message, count(liker) as likes, liked