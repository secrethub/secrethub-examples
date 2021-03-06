resource "secrethub_service" "demo_service_account" {
  repo = "${var.secrethub_username}/${var.repo_name}"
}

resource "secrethub_access_rule" "demo_access_rule" {
  account_name = secrethub_service.demo_service_account.id
  dir          = "${var.secrethub_username}/${var.repo_name}"
  permission   = "read"
}

resource "secrethub_secret" "test_secret" {
  path = "${var.secrethub_username}/${var.repo_name}/test"

  generate {
    length   = 22
  }
}
