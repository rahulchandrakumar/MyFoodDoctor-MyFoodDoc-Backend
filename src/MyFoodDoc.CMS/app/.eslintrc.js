module.exports = {
  root: true,
  env: {
    node: true
  },
  extends: ["plugin:vue/recommended"],
  rules: {
    "no-console": process.env.NODE_ENV === "production" ? "error" : "off",
    "no-debugger": process.env.NODE_ENV === "production" ? "error" : "off",
    "vue/max-attributes-per-line": ["error", {
      "singleline": {
        "max": 3,
      },
      "multiline": {
        "max": 3,
        "allowFirstLine": false,
      }
    }]
  },
  parserOptions: {
    parser: "babel-eslint"
  }
};
