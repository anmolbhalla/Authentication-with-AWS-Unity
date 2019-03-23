const app = require("express")();
const bodyparser = require("body-parser");
const mongoose = require("mongoose");
const clear = require('clear');

var call = 0;

const dbOptions = { useNewUrlParser: true, reconnectTries: Number.MAX_VALUE, poolSize: 10 };
mongoose.connect(require("./mongo"), dbOptions).then(
    () => { console.log(">  Connection Established"); },
    e => { console.log(">  Connection Failed \n>  " + e); }
);

app.use(bodyparser.urlencoded({ extended: true }));
app.use(function(req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
});

var student = mongoose.model("student", new mongoose.Schema({
    username: String,
    email: String,
    pass: String,
    name: String,
    marks: String,
    mobile: String,
    teachers: [String]
}));
var teach = mongoose.model("teach", new mongoose.Schema({
    username: String,
    email: String,
    pass: String,
    name: String,
    mobile: String
}));

app.post("/login", function(req, res) {
    var username = req.body.username;
    var pass = req.body.pass;
    var type = req.body.type;
    console.log("\n" + ++call + ") Authentication Started");
    if (type == "student") {
        student.find({ username: username }, function(e, user) {
            if (e) {
                console.log(">  Error occured while logging in :\n>  " + e);
                res.send("0");
            }
            else if (user.length > 0) {
                if (user[0].pass == pass) {
                    res.json(user);
                    console.log(">  Student's Authentication Successfull");
                }
                else {
                    res.send("0");
                    console.log(">  Student's Authentication Terminated : Invalid Password");
                }
            }
            else if (user.length <= 0) {
                res.send("2");
                console.log(">  Student's Authentication Terminated : User doesn't exist");
            }
        });
    }
    else if (type == "teacher") {
        teach.find({ username: username }, function(e, user) {
            if (e) {
                console.log(">  Error occured while logging in :\n>  " + e);
                res.send("0");
            }
            else if (user.length > 0) {
                if (user[0].pass == pass) {
                    res.json(user);
                    console.log(">  Teacher's Authentication Successfull");
                }
                else {
                    res.send("0");
                    console.log(">  Teacher's Authentication Terminated : Invalid Password");
                }
            }
            else if (user.length <= 0) {
                res.send("2");
                console.log(">  Teacher's Authentication Terminated : User doesn't exist");
            }
        });
    }
});
app.post("/signup", function(req, res) {
    var email = req.body.email,
        username = req.body.username,
        pass = req.body.pass,
        name = req.body.name,
        marks = Math.floor(Math.random() * (100 - 25 + 1) + 25),
        mobile = req.body.mobile,
        teachers = req.body.teachers,
        type = req.body.type;
    console.log("\n" + ++call + ") Profile Creation Started");
    if (type == "student") {
        teachers = teachers.split(",");
        student.create({
            username: username,
            email: email,
            pass: pass,
            name: name,
            marks: marks,
            mobile: mobile,
            teachers: teachers
        }, function(e) {
            if (e) {
                res.send("0");
                console.log(">  Error While Student's Creating Account\n>  " + e);
            }
            else {
                console.log(">  Student's Account Created Successfuly");
                res.send("1");
            }
        });
    }
    else if (type == "teacher") {
        teach.create({
            username: username,
            email: email,
            pass: pass,
            name: name,
            mobile: mobile,
        }, function(e) {
            if (e) {
                res.send("0");
                console.log(">  Error While Creating Teacher's  Account\n>  " + e);
            }
            else {
                console.log(">  Teacher's Account Created Successfuly");
                res.send("1");
            }
        });
    }
});
app.post("/teachers", function(req, res) {
    var name = req.body.name;
    console.log(name);
    console.log("\n" + ++call + ") Teacher's Assigned Students Call");
    student.find({}, function(err, users) {
        if (err) {
            console.log(err);
        }
        else {
            var students = {};
            users.forEach(function(user) {
                (user.teachers).forEach(function(teacher) {
                    if (teacher + "" == name + "")
                        students[user.name] = user.marks;
                });
            });
            res.json(students);
        }
    });
});

app.get("*", function(req, res) {
    res.send("Working!!!");
});

app.listen(8080, function() {
    clear();
    console.log("\n" + ++call + ") Starting Server");
    console.log(">  Server is Listening");
    console.log("\n" + ++call + ") Connection to MongoDB Atlas Server");
});
