import * as functions from 'firebase-functions';
import * as admin from 'firebase-admin';
import * as cors from 'cors';


const corsHandler = cors({
    origin: true,
    maxAge: 600,
});

admin.initializeApp();

const db = admin.firestore();

/*
const now = function () {
    return admin.firestore.FieldValue.serverTimestamp();
}*/

export const add_user = functions.https.onRequest((request, response) => {

    corsHandler(request, response, () => {
		//request.body = {id, name, score}

        db.collection("users").doc(request.body.id).set({
            name: request.body.name,
			score: request.body.score
        })
        .then(function () {
            console.log("User successfully added!");
            response.json([])
        })
        .catch(function (error) {
            console.error("Error adding user: ", error);
            response.json(error);
        });
    });
});



export const get_users = functions.https.onRequest((request, response) => {
    corsHandler(request, response, () => {
		//request.body = {ids}

        const ids = request.body.ids;        
		var friends: { [id: string] : object; } = {};

		db.collection('users').get()
		.then(
			(results) => {
				results.forEach((doc) => {
					if(ids.includes(doc.id)){
						friends[doc.id] = doc.data();
					}
				});
				console.log("Friends retrieved successfully!");
				response.json(friends);
			}			
		)
		.catch(function (error) {
            console.error("Error retrieving friends: ", error);
            response.send(error)
        });
    });
});



/*
const getField = function (email, fieldName): any {
    db.collection("users").doc(email).get()
        .then(
            (doc) => {
                if (doc.exists) {
                    const d = doc.data();
                    if (d[fieldName]) return d[fieldName];
                    else return null;
                }
                else {
                    return null;
                }
            }
        )
        .catch(function (error) {
            return null;
        });
}

const setUser = function (email, name, email_verified, status, last_login, begin_trial, code) {
    db.collection("users").doc(email).set({
        name: name,
        auth_code: code,
        email_verified: email_verified,
        email: email,
        begin_trial: begin_trial,
        status: status,
        last_login: last_login
    })
        .then(function () {
            //response.json("[setUser] SUCCESS");
            console.log("[setUser] SUCCESS");
            return true;
        })
        .catch(function (error) {
            //response.json("[setUser] FAILED");
            console.error("[setUser] FAILED: ", error);
            return false;
        });
};

const now = function () {
    return admin.firestore.FieldValue.serverTimestamp();
}

export const auth_user = functions.https.onRequest((request, response) => {
    corsHandler(request, response, () => {
        //request.body = {email,code}

        //ENVIANDO EMAIL
        const email = request.body.email.toLowerCase();

        console.log(email);

        const APP_NAME = "Mundo Game Station"
        const auth_code = request.body.auth_code;
        const subject = `Bem vindo ao ${APP_NAME}!`;


        //response.send("Email sent")


        //AUTENTICANDO EMAIL
        db.collection("users").doc(email).get()
            .then(
                (doc) => {
                    let docExist = false;
                    if (doc.exists) {
                        console.log("[auth_user] Autenticando usuario que ja existe");
                        const d = doc.data();
                        //d.begin_trial = new Date(d.begin_trial).toString();
                        //d.last_login = new Date(d.last_login).toString();
                        setUser(email, d.name, d.email_verified, d.status, now(), d.begin_trial, auth_code);
                        docExist = true;
                    }
                    else {
                        console.log("[auth_user] Autenticando usuario novo");
                        setUser(email, "Jeronimo", false, "unverified", now(), now(), auth_code);
                    }

                    let text = getEmailBody(email, auth_code, docExist);
                    sendEmail(email, subject, text)
                    response.json([]);
                }
            )
            .catch(function (error) {
                console.error("[auth_user] Error getting user: ", error);
                response.send(error)
            });
    });
});




export const add_user = functions.https.onRequest((request, response) => {

    corsHandler(request, response, () => {

        //const now = admin.firestore.FieldValue.serverTimestamp();
        //request.body = {email, status}
        const email = request.body.email.toLowerCase();
        console.log("received email: " + email);
        const us_status = request.body.status ? request.body.status : "trial"
        db.collection("users").doc(email).set({
            name: "Jeronimo",
            auth_code: "ABC123",
            email_verified: false,
            email: email,
            begin_trial: now(),
            status: us_status,
            last_login: now()
        })
            .then(function () {
                console.log("User successfully added!");
                response.json([])
            })
            .catch(function (error) {
                console.error("Error adding user: ", error);
                response.json(error);
            });
    });
});

export const get_user = functions.https.onRequest((request, response) => {
    corsHandler(request, response, () => {
        //request.body = {email}
        const email = request.body.email.toLowerCase();
        console.log(email);
        db.collection("users").doc(email).get()
            .then(
                (doc) => {
                    if (doc.exists) {
                        const d = doc.data();
                        d.begin_trial = new Date(d.begin_trial).toString();
                        d.last_login = new Date(d.last_login).toString();
                        response.json(d);
                    }
                    else {
                        response.json([]);
                    }
                }
            )
            .catch(function (error) {
                console.error("Error getting user: ", error);
                response.send(error)
            });
    });
});

export const update_user = functions.https.onRequest((request, response) => {
    corsHandler(request, response, () => {

        const email = request.body.email.toLowerCase();

        let currentStatus = "";

        //request.body = {email, status}
        db.collection("users").doc(email).get().then(
            (doc) => {
                if (doc.exists) {
                    const d = doc.data();
                    currentStatus = d.status;
                }
            }
        )
            .catch(function (error) {
                console.error("Error getting user: ", error);
            });

        if (currentStatus === "signed" && (request.body.status === "trial" || request.body.status === "expired")) {
            currentStatus = "canceled";
        }
        else currentStatus = request.body.status;

        //const now = admin.firestore.FieldValue.serverTimestamp();

        db.collection("users").doc(email).update({
            status: currentStatus,
            last_login: now()
        })
            .then(function () {
                console.log("Status successfully updated!");
                response.send([]);
            })
            .catch(function (error) {
                console.error("Error updating status: ", error);
                ;
                response.json([])
            });
    });

});

export const verify_email = functions.https.onRequest((request, response) => {

    corsHandler(request, response, () => {
        //request.body = {email, code}
        const email = request.body.email.toLowerCase();
        console.log(request.params);

        db.collection("users").doc(email).get()
            .then(
                (doc) => {
                    if (doc.exists) {
                        const d = doc.data();

                        if (d.code === request.body.code.toUpperCase()) {
                            setUser(d.email, d.name, true, "verified", now(), d.begin_trial, d.code)
                            const answer = {
                                "confirmed": true
                            }
                            response.json(answer);
                        }

                        else {
                            const answer = {
                                "confirmed": false
                            }
                            response.json(answer);
                        }
                    }
                    else {
                        const answer = {
                            "confirmed": false
                        }
                        response.json(answer);
                    }
                }
            )
            .catch(function (error) {
                console.error("Error getting user: ", error);
                response.json(false);
            });
    });
});


export const get_active_users = functions.https.onRequest((request, response) => {

    corsHandler(request, response, () => {

        const oneMonthAgo = new Date();
        oneMonthAgo.setDate(oneMonthAgo.getDate() - 30);

        const answer = [];

        db.collection("users")
            //.where("last_login", ">=", oneMonthAgo)
            //.where("status", "==", "signed")
            .get().then(
                (querySnapshot) => {
                    querySnapshot.forEach(
                        (doc) => {
                            const d = doc.data();
                            //const date = new Date(d.trial);
                            answer.push(d);
                            console.log("ACHOU ALGUEM");
                        }
                    )

                    try {
                        const opt = {
                            fields: [
                                "email", "name", "status", "email_verified", "begin_trial"
                            ]
                        }
                        const csv = json2csv(answer, opt);
                        response.setHeader(
                            "Content-disposition",
                            "attachment; filename=report.csv"
                        )
                        response.set("Content-Type", "text/csv")
                        response.status(200).send(csv)
                    }
                    catch (e) {
                        response.json(e);
                        console.error("error: ", e);
                    }


                }
            )
            .catch(function (error) {
                response.json([]);
                console.error("Error getting user: ", error);
            });



    });
});



function sendEmail(emailTo, subject, html) {
    const mailTransport = nodemailer.createTransport({
        
        host: "smtp.dreamhost.com",
        port: "587",
        secure: false,
        auth: {
            user: "info@mundogamestation.com",
            pass: "!Mu6Zp5k",
        },
    });
    const mailOptions = {
        from: "<info@mundogamestation.com>",
        to: emailTo,
        subject: subject,
        html: html,
    };
    try {
        mailTransport.sendMail(mailOptions);
        console.log(`email sent to:`, emailTo);
    } catch (error) {
        console.error('There was an error while sending the email:', error);
    }
}

*/