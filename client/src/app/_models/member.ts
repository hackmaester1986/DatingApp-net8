import { Photo } from "./photo"

export interface Member {
    id: number
    username: string
    age: number
    photoUrl: string
    knownAs: string
    created: Date
    lastActive: Date
    introduction: string
    gender: string
    interests: string
    lookinFor: any
    city: string
    country: string
    photos: Photo[]
  }
  
