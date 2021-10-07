How it design?
The basic idea of the whole program is tring to follow MVVM which sparate a program to model, viewmodel and view which make the code more readable and maintainbale.
The view and viewmodel part is every window.
The model is the Pokemon file which contain the little database and some basic logic for all windows.
The Mainwindow's reposibility is moving the Player and call other windows.
And similarly all other windows' responsibility are managing their own function.

Design Pattern
Singleton
The Player class is a singleton class that make sure every class can get only one player class without worring the player could be null.

Factory
The Pokemon and Potion class both have a Factory that can return the object or the player can get a object.

S.O.L.I.D. Principles
We are also tring to follow the S.O.L.I.D. Principles.

Single Responsibility Principle
Basically all function will only have only one function.

Open/Close Principle
We are tring make the function can be open and close, for example, a basic method like Shake(dynamic obj) which you can perform shake animation with any object with RenderTransform property.

Class Reuse
As the program separate two part so all common class in Pokemon file and the PokemonGame use the class from pokemon and have its own class the represent their own view.

Data structure
Since we only use a few of pokemon type so we don't separate the database from model.

Using Dictionary<string, LinkedList> PokemonTypeTable as a pokemon evolve table is more maintainable and extendable, in this data structure the programmer can always get or add the evolve type without worring the future evolve path, and if someday a pokemon get more than one evolve path we can still achieve it whithout change anything.

In the Player class, we are using Dictionary<int, Pokemon> pokemons that int is the unique ID of the pokemon which can identify which pokemon we want to manipulate. And also it is easily and quickly to add and remove by Dictionary.

Challenges overcome
The main problem is cooperation since it is not easy let the others doing what you wish but the good thing the program can be separate by its function so when we are doing something wrong, we are only affect ourself instead of ruining the whole program.
And also if we want merge the program or use the function from the others we can just merge it and use the function from Pokemon file.
