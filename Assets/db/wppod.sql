LOCK TABLES `Menu` WRITE;
ALTER TABLE `Menu` DISABLE KEYS;
INSERT INTO `Menu` VALUES (1,'Breakfast'),(2,'Lunch');
ALTER TABLE `Menu` ENABLE KEYS;
UNLOCK TABLES;

LOCK TABLES `MenuItem` WRITE;
ALTER TABLE `MenuItem` DISABLE KEYS;
INSERT INTO `MenuItem` VALUES (1,'Carefully roasted best coffees in the world using innovative and methodic roasting practices.',0,1,'https://raw.githubusercontent.com/inkton/sample.wppod/master/Assets/images/menu/breakfast-coffee.jpg',8.000000000000000000000000000000,'Coffee'),(2,'Slices of bread with a light layer of mayo with a slice of cheese on one piece of bread and tomato slices sprinkled with salt, pepper & Italian seasoning.',1,1,'https://raw.githubusercontent.com/inkton/sample.wppod/master/Assets/images/menu/breakfast-cheese-tomato.jpg',10.000000000000000000000000000000,'C & T Sandwich'),(3,'Witlof and apple salad stirred with walnut oil dressing and garnished with cheese.',1,1,'https://raw.githubusercontent.com/inkton/sample.wppod/master/Assets/images/menu/breakfast-cheese-salad.jpg',10.000000000000000000000000000000,'Cheese Salad'),(4,'A caesar with lettuce, croutons and the rich signature dressing with lettuce and parmesan on a serving platter.',2,2,'https://raw.githubusercontent.com/inkton/sample.wppod/master/Assets/images/menu/lunch-caesars-salad.jpg',10.000000000000000000000000000000,'Caesar Salad'),(5,'Spinach leaves, goat cheese and chopped parsley in a large bowl, whisked with lemon juice sea salt and freshly ground black pepper.',2,2,'https://raw.githubusercontent.com/inkton/sample.wppod/master/Assets/images/menu/lunch-goat-cheese-salad.jpg',10.000000000000000000000000000000,'Goat Cheese Salad'),(6,'Barbecued cauliflower and chickpea falafel veggie burgers topped with strawberry barbecue sauce.',1,2,'https://raw.githubusercontent.com/inkton/sample.wppod/master/Assets/images/menu/lunch-vege-burger.jpg',8.000000000000000000000000000000,'Vege Burger');
ALTER TABLE `MenuItem` ENABLE KEYS;
UNLOCK TABLES;


LOCK TABLES `Stock` WRITE;
ALTER TABLE `Stock` DISABLE KEYS ;
INSERT INTO `Stock` VALUES (1,'Coffee'),(2,'Sugar'),(3,'Milk'),(4,'Bread'),(5,'Butter'),(6,'Cheese'),(7,'Lettuce'),(8,'Dressing'),(9,'Tomato');
ALTER TABLE `Stock` ENABLE KEYS ;
UNLOCK TABLES;
