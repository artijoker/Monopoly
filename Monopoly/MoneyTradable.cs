
namespace Monopoly {
    class MoneyTradable : ITradable {
        public MoneyTradable(int money) {
            Property = null;
            Money = new Optional<int>(money);
        }
        public Property Property { get; }
        public Optional<int> Money { get; }
    }
}
