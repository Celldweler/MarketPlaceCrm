<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <v-card>
        <v-btn @click="printToConsole()">notifications: {{ notifications.length }}</v-btn>
        <v-card-title class="headline">
          {{ message }}
        </v-card-title>
        <v-card-text>
          <div v-for="m in Messages">
            {{ m.message }}
          </div>
          <v-text-field label="send comment" v-model="messageContent"/>
          <v-btn @click="sendComment">send</v-btn>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script>
import {HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr"

export default {
  name: 'IndexPage',
  data: () => ({
    notifications: [],
    message: "init",
    connection: null,
    messageContent: "",
    name: "",
    users: [{name: "Test User"}],
    Messages: [{message: "test: from test"}],
  }),
  methods: {
    printToConsole() {
      console.log("notifications: ", this.notifications);
    },

    sendComment() {
      this.$axios.$post("/api/comments", {
        text: this.messageContent,
        customerId: 1,
        productId: 1,
        connectionId: this.connection.connectionId
      })
        .then(res => console.log(res))
        .catch(er => console.log(res))
    },
    sendMessage() {
      this.$axios.$get("https://localhost:5001/api/test/send?msg=" + this.messageContent)
        .then(res => {
          // console.log(res)
        })
        .catch(er => console.log(er))
        .finally(x => {
          this.messageContent = "";
        })
      // var res = this.connection.invoke("SendMessage", this.messageContent).catch(function (err) {
      //   return console.error(err);
      // });
    },
    joinRoom() {
      this.dialog = false;
      this.connection.invoke("JoinRoom", this.name).catch(function (err) {
        return console.error(err);
      });
    },
    listen() {
      window.console.log("Listen Started");
      if (this.connection.state !== HubConnectionState.Connected) {
        this.connect().finally(() => {
          this.listen();
          return;
        });
      }
      this.connection.on("NewConnection", (res) => {
        console.log(res);
      });
      this.connection.on("onNewNotification", (res) => {
        console.log(res);
      });
      this.connection.on("JoinRoom", (res) => {
        var userObj = {
          name: res
        };
        this.users.push(userObj);
      });
      this.connection.on("SendMessage", (res) => {
        var messageObj = {
          message: res
        };
        this.Messages.push(messageObj);
        console.log(res);
      })
    },

  },
  async created() {
    // this.$axios.$get("https://localhost:5001/api/test/send?msg=test-message")
    //   .then(res => {
    //     console.log(res)
    //   })
    this.$axios.$get("api/notifications/1").then(res => {
      console.log(res)
      this.notifications = res;
    }).catch(er => console.log(er))


    this.message = (await this.$axios.$get("/api/test"));
    if (this.connection === null) {
      this.connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5000/customHub")
        .build()
    }
    this.connection
      .start()
      .then(() => {
        window.console.log("Connection Success", this.connection.connectionId);

        this.listen();
      })
      .catch((err) => {
        window.console.log(`Connection Error ${err}`);
      });
    this.connection.onclose(() => {
      window.console.log("Connection Destroy");
    });
  }
}
</script>
