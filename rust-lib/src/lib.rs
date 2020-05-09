pub fn reverse(input: &str) -> String {
    return input.chars().rev().collect::<String>();
}

#[cfg(test)]
mod tests {
    #[test]
    fn it_works() {
        assert_eq!(crate::reverse("farp"), "praf");
    }
}
