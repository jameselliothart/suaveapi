namespace suaveapi.Db
open System.Collections.Generic

type Person = {
    Id : int
    Name : string
    Age : int
    Email : string
}

module Db =
    let private peopleStorage = new Dictionary<int, Person>()
    let getPeople () =
        peopleStorage.Values |> Seq.map id

    let createPerson person =
        let id = peopleStorage.Values.Count + 1
        let newPerson = {
            Id = id
            Name = person.Name
            Age = person.Age
            Email = person.Email
        }
        peopleStorage.Add(id, newPerson)
        newPerson

    let updatePersonById personId personToBeUpdated =
        match peopleStorage.TryGetValue(personId) with
        | (true, person) ->
            let updatedPerson = { person with Name = personToBeUpdated.Name; Age = personToBeUpdated.Age; Email = personToBeUpdated.Email}
            peopleStorage.[personId] <- updatedPerson
            Some updatedPerson
        | (false, _) -> None

    let updatePerson personToBeUpdated =
        updatePersonById personToBeUpdated.Id personToBeUpdated

    let deletePerson personId =
        peopleStorage.Remove(personId) |> ignore