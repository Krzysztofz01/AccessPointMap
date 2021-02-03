db.auth('mongoroot', "Mypassword123");

db.createUser({
    user: "LogDbAdmin",
    pwd: "Mypassword123",
    roles: [{
        role: "readWrite",
        db: "LogDb"
    }]
});