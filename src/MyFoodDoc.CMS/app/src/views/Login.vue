<template>
  <v-container fluid fill-height>
    <v-layout align-center justify-center>
      <v-flex xs12 sm8 md4>
        <material-card color="green" title="Login">
          <v-form>
            <v-layout wrap>
              <v-flex xs12>
                <v-text-field
                  v-model="loginForm.username"
                  label="Login"
                  name="login"
                  prepend-icon="mdi-account"
                  type="text"
                  required
                  @keypress.enter="login()"
                />
              </v-flex>

              <v-flex xs12>
                <v-text-field
                  id="password"
                  v-model="loginForm.password"
                  label="Password"
                  name="password"
                  prepend-icon="mdi-lock"
                  type="password"
                  required
                  @keypress.enter="login()"
                />
              </v-flex>

              <v-flex v-if="loginError != null" xs12>
                {{ loginError }}
              </v-flex>

              <v-flex xs12>
                <v-btn
                  color="success" @click="login()"
                >
                  Login
                </v-btn>
              </v-flex>
            </v-layout>
          </v-form>
        </material-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import Vue from "vue";

export default Vue.extend({
  name: "Login",
  data() {
    return {
      loginForm: {
        username: "",
        password: ""
      },
      hasFailed: false
    };
  },
  computed: {
    loginError() {
      const message = "Wrong username or password";
      return this.hasFailed ? message : null;
    }
  },
  methods: {
    async login() {
      try {
        const success = await this.$store.dispatch("user/login", this.loginForm);
        if (success) {
          this.loginForm.username = "";
          this.loginForm.password = "";
          this.$router.push({ name: "Portions" });
        } else {
          this.hasFailed = true;
        }
      } catch (ex) {
        this.hasFailed = true;
      }
    },
    reset() {
      this.loginForm.username = "";
      this.loginForm.password = "";
    }
  }
});
</script>
